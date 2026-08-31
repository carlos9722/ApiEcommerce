using ApiEcommerce.Constans;
using ApiEcommerce.Data;
using ApiEcommerce.Repository;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;


// Punto de entrada y configuración principal de la aplicación.
var builder = WebApplication.CreateBuilder(args);

// Obtiene la cadena de conexión configurada en appsettings.json.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

// Registra el DbContext y configura SQL Server como proveedor de base de datos.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

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

// Registra los Controllers para manejar las peticiones HTTP.
builder.Services.AddControllers();

// Habilita la generación de documentación OpenAPI.
builder.Services.AddOpenApi();

// Configura las políticas de CORS (Cross-Origin Resource Sharing),
// permitiendo controlar qué aplicaciones externas pueden consumir la API.
builder.Services.AddCors(options =>
  {
    options.AddPolicy(PolicyNames.AllowSpecificOrigin,
    builder =>
    {
      builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
    }
    );
  }
);

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

// Habilita el middleware de autorización.
app.UseAuthorization();

// Conecta los Controllers con las rutas de la aplicación.
app.MapControllers();

// Inicia la aplicación.
app.Run();