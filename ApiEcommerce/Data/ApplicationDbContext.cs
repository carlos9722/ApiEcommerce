using ApiEcommerce.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Data
{
    /// <summary>
    /// Contexto principal de acceso a datos de la aplicación.
    /// 
    /// <para>
    /// Hereda de <see cref="IdentityDbContext{TUser}"/>, que extiende
    /// <see cref="DbContext"/> y proporciona la infraestructura necesaria
    /// para trabajar con Entity Framework Core y ASP.NET Core Identity.
    /// </para>
    /// 
    /// <para>
    /// Utiliza <see cref="ApplicationUser"/> como entidad de usuario personalizada,
    /// permitiendo administrar usuarios, roles y demás información relacionada
    /// con el sistema de identidad de la aplicación.
    /// </para>
    /// 
    /// <para>
    /// A través de este contexto podemos consultar, agregar, modificar y eliminar
    /// información de las entidades que forman parte de la aplicación.
    /// </para>
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        /// <summary>
        /// Inicializa el contexto de base de datos utilizando la configuración
        /// proporcionada por Entity Framework Core.
        /// 
        /// <para>
        /// <see cref="DbContextOptions{TContext}"/> contiene las opciones necesarias
        /// para configurar este contexto, como el proveedor de base de datos
        /// que se utilizará y la cadena de conexión.
        /// </para>
        /// 
        /// <para>
        /// El parámetro <paramref name="options"/> se recibe normalmente mediante
        /// inyección de dependencias desde la configuración de la aplicación.
        /// </para>
        /// 
        /// <para>
        /// <c>base(options)</c> envía estas opciones al constructor de la clase
        /// base <see cref="IdentityDbContext{TUser}"/>. De esta forma, Entity Framework Core
        /// recibe la configuración necesaria para trabajar con la base de datos
        /// y ASP.NET Core Identity.
        /// </para>
        /// </summary>
        /// <param name="options">
        /// Configuración de Entity Framework Core para este contexto.
        /// </param>
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// Configura el modelo de datos que utilizará Entity Framework Core.
        /// 
        /// <para>
        /// Este método permite establecer configuraciones adicionales sobre
        /// las entidades, propiedades, relaciones y restricciones del modelo
        /// antes de que Entity Framework Core genere o utilice la estructura
        /// correspondiente en la base de datos.
        /// </para>
        /// 
        /// <para>
        /// <c>base.OnModelCreating(modelBuilder)</c> mantiene las configuraciones
        /// predeterminadas proporcionadas por ASP.NET Core Identity, incluyendo
        /// las entidades relacionadas con usuarios, roles y autenticación.
        /// </para>
        /// </summary>
        /// <param name="modelBuilder">
        /// Constructor utilizado para configurar el modelo de datos
        /// de Entity Framework Core.
        /// </param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Representa el conjunto de categorías que Entity Framework Core
        /// administra en la base de datos.
        /// 
        /// <para>
        /// El nombre <c>Categories</c> representa una colección de objetos
        /// <see cref="Category"/>. Entity Framework Core utiliza este
        /// <see cref="DbSet{TEntity}"/> para realizar operaciones sobre
        /// los registros correspondientes a la entidad <see cref="Category"/>.
        /// </para>
        /// 
        /// <para>
        /// Por medio de <c>Categories</c> podemos consultar, agregar, actualizar
        /// y eliminar categorías utilizando C# y Entity Framework Core,
        /// sin tener que escribir directamente las consultas SQL para estas
        /// operaciones.
        /// </para>
        /// </summary>
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        /// Representa el conjunto de productos que Entity Framework Core
        /// administra en la base de datos.
        /// 
        /// <para>
        /// El nombre <c>Products</c> representa una colección de objetos
        /// <see cref="Product"/>. Entity Framework Core utiliza este
        /// <see cref="DbSet{TEntity}"/> para realizar operaciones sobre
        /// los registros correspondientes a la entidad <see cref="Product"/>.
        /// </para>
        /// 
        /// <para>
        /// Por medio de <c>Products</c> podemos consultar, agregar, actualizar
        /// y eliminar productos utilizando C# y Entity Framework Core,
        /// sin tener que escribir directamente las consultas SQL para estas
        /// operaciones.
        /// </para>
        /// </summary>
        public DbSet<Product> Products { get; set; }

        /// <summary>
        /// Representa el conjunto de usuarios que Entity Framework Core
        /// administra en la base de datos.
        /// 
        /// <para>
        /// El nombre <c>Users</c> representa una colección de objetos
        /// <see cref="User"/>. Entity Framework Core utiliza este
        /// <see cref="DbSet{TEntity}"/> para realizar operaciones sobre
        /// los registros correspondientes a la entidad <see cref="User"/>.
        /// </para>
        /// 
        /// <para>
        /// Por medio de <c>Users</c> podemos consultar, agregar, actualizar
        /// y eliminar usuarios utilizando C# y Entity Framework Core,
        /// sin tener que escribir directamente las consultas SQL para estas
        /// operaciones.
        /// </para>
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// Representa el conjunto de usuarios personalizados que Entity Framework Core
        /// administra en la base de datos mediante ASP.NET Core Identity.
        /// 
        /// <para>
        /// El nombre <c>ApplicationUsers</c> representa una colección de objetos
        /// <see cref="ApplicationUser"/>. Esta entidad hereda de
        /// <see cref="Microsoft.AspNetCore.Identity.IdentityUser"/> y permite
        /// agregar información personalizada a los usuarios administrados por
        /// ASP.NET Core Identity.
        /// </para>
        /// 
        /// <para>
        /// Por medio de <c>ApplicationUsers</c> podemos consultar, agregar, actualizar
        /// y eliminar usuarios personalizados utilizando C# y Entity Framework Core.
        /// </para>
        /// </summary>
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
