using System.Text;
using ApiEcommerce.Constans;
using ApiEcommerce.Constants;
using ApiEcommerce.Data;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;


// Punto de entrada y configuración principal de la aplicación.
var builder = WebApplication.CreateBuilder(args);

// Obtiene la cadena de conexión configurada en appsettings.json.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

// Registra el DbContext y configura SQL Server como proveedor de base de datos.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

// Configura el servicio de caché de respuestas HTTP.
builder.Services.AddResponseCaching(options =>
{
  options.MaximumBodySize = 1024 * 1024;
  options.UseCaseSensitivePaths = true;
});

// Registra el repositorio de categorías con ciclo de vida Scoped,
// creando una instancia por cada solicitud HTTP.
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Registra el repositorio de productos con ciclo de vida Scoped,
// creando una instancia por cada solicitud HTTP.
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Registra el repositorio de usuarios con ciclo de vida Scoped,
// creando una instancia por cada solicitud HTTP.
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Registra AutoMapper y carga los perfiles de mapeo definidos
// en el ensamblado donde se encuentra la clase Program.
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// Obtiene la clave secreta utilizada para firmar y validar los tokens JWT
// desde la configuración de la aplicación.
var secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");

// Verifica que la clave secreta JWT esté configurada correctamente.
if (string.IsNullOrEmpty(secretKey))
{
    throw new InvalidOperationException("SecretKey no esta configurada");
}

// Configura el sistema de autenticación de la aplicación utilizando JWT Bearer
// como esquema predeterminado para autenticar las solicitudes.
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Permite utilizar HTTP durante el desarrollo sin exigir HTTPS para los metadatos.
    options.RequireHttpsMetadata = false;

    // Permite conservar el token recibido dentro del contexto de autenticación.
    options.SaveToken = true;

    // Configura los parámetros utilizados para validar los tokens JWT.
    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Valida que la firma del token sea correcta.
        ValidateIssuerSigningKey = true,

        // Utiliza la clave secreta configurada para comprobar la firma del token.
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        ),

        // No valida el emisor del token.
        ValidateIssuer = false,

        // No valida la audiencia del token.
        ValidateAudience = false
    };
});

// Registra los Controllers y configura los perfiles de caché disponibles.
builder.Services.AddControllers(option =>
{
  option.CacheProfiles.Add(CacheProfiles.Default10, CacheProfiles.Profile10);
  option.CacheProfiles.Add(CacheProfiles.Default20, CacheProfiles.Profile20);
}
);

// Habilita la generación de documentación OpenAPI.
builder.Services.AddOpenApi(options =>
{
    // Agrega la definición de seguridad JWT Bearer al documento OpenAPI,
    // permitiendo que Swagger conozca el mecanismo de autenticación utilizado por la API.
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            // Inicializa los componentes del documento OpenAPI si todavía no existen.
            document.Components ??= new OpenApiComponents();

            // Inicializa la colección de esquemas de seguridad si todavía no existe.
            document.Components.SecuritySchemes ??=
                new Dictionary<string, IOpenApiSecurityScheme>();

            // Registra el esquema de autenticación JWT Bearer.
            document.Components.SecuritySchemes["Bearer"] =
                new OpenApiSecurityScheme
                {
                    // Descripción que se mostrará en Swagger al configurar la autenticación.
                    Description =
                        "Nuestra API utiliza la Autenticación JWT usando el esquema Bearer. \n\r\n\r" +
                        "Ingresa el token generado en login.\n\r\n\r" +
                        "Ejemplo: \"12345abcdef\"",

                    // Nombre del encabezado HTTP donde se enviará el token.
                    Name = "Authorization",

                    // Indica que el token se enviará mediante los encabezados HTTP.
                    In = ParameterLocation.Header,

                    // Define el tipo de esquema de autenticación como HTTP.
                    Type = SecuritySchemeType.Http,

                    // Especifica que se utilizará el esquema Bearer.
                    Scheme = "bearer",

                    // Indica que el formato del token utilizado es JWT.
                    BearerFormat = "JWT"
                };

            // Recorre todas las rutas definidas en el documento OpenAPI.
            foreach (var path in document.Paths.Values)
            {
                // Verifica que la ruta tenga operaciones HTTP definidas.
                if (path.Operations is null)
                {
                    continue;
                }

                // Recorre todas las operaciones HTTP disponibles para cada ruta.
                foreach (var operation in path.Operations.Values)
                {
                    // Inicializa la colección de requisitos de seguridad
                    // si todavía no existe.
                    operation.Security ??=
                        new List<OpenApiSecurityRequirement>();

                    // Indica que la operación utiliza el esquema de autenticación Bearer.
                    operation.Security.Add(
                        new OpenApiSecurityRequirement
                        {
                            [
                                new OpenApiSecuritySchemeReference(
                                    "Bearer",
                                    document
                                )
                            ] = []
                        }
                    );
                }
            }

            // Finaliza la transformación del documento OpenAPI.
            return Task.CompletedTask;
        }
    );
});

// Configura el versionado de la API y define la versión predeterminada.
// Si el cliente no especifica una versión, se utilizará la versión 1.0.
var apiVersioningBuilder = builder.Services.AddApiVersioning(option =>
{
  option.AssumeDefaultVersionWhenUnspecified = true;
  option.DefaultApiVersion = new ApiVersion(1, 0);
  option.ReportApiVersions = true;
  // option.ApiVersionReader = ApiVersionReader.Combine(new QueryStringApiVersionReader("api-version")); //?api-version
});
apiVersioningBuilder.AddApiExplorer(option =>
{
  option.GroupNameFormat = "'v'VVV"; // v1,v2,v3...
  option.SubstituteApiVersionInUrl = true; // api/v{version}/products
});

// Configura las políticas de CORS (Cross-Origin Resource Sharing),
// permitiendo controlar qué aplicaciones externas pueden consumir la API.
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        PolicyNames.AllowSpecificOrigin,
        builder =>
        {
            builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
        }
    );
});

// Construye la aplicación con los servicios configurados.
var app = builder.Build();

// Configuración del pipeline HTTP.
if (app.Environment.IsDevelopment())
{
    // Habilita el documento OpenAPI.
    app.MapOpenApi();

    // Habilita la interfaz visual de Swagger.
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API V1");
    });
}

// Redirige las peticiones HTTP hacia HTTPS.
app.UseHttpsRedirection();

// Aplica la política de CORS configurada anteriormente,
app.UseCors(PolicyNames.AllowSpecificOrigin);

// Habilita el middleware de caché de respuestas HTTP.
app.UseResponseCaching();

// Habilita el middleware de autenticación,
// encargado de validar las credenciales del usuario, incluyendo los tokens JWT.
app.UseAuthentication();

// Habilita el middleware de autorización,
// encargado de determinar si el usuario autenticado tiene permiso para acceder al recurso.
app.UseAuthorization();

// Conecta los Controllers con las rutas de la aplicación.
app.MapControllers();

// Inicia la aplicación.
app.Run();
