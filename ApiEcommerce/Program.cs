using ApiEcommerce.Data;
using Microsoft.EntityFrameworkCore;


// Punto de entrada y configuración principal de la aplicación.
var builder = WebApplication.CreateBuilder(args);

// Obtiene la cadena de conexión configurada en appsettings.json.
var dbConnectionString = builder.Configuration.GetConnectionString("ConexionSql");

// Registra el DbContext y configura SQL Server como proveedor de base de datos.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

// Registra los Controllers para manejar las peticiones HTTP.
builder.Services.AddControllers();

// Habilita la generación de documentación OpenAPI.
builder.Services.AddOpenApi();

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

// Habilita el middleware de autorización.
app.UseAuthorization();

// Conecta los Controllers con las rutas de la aplicación.
app.MapControllers();

// Inicia la aplicación.
app.Run();