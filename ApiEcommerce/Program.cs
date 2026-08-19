// Punto de entrada y configuración principal de la aplicación.
var builder = WebApplication.CreateBuilder(args);

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