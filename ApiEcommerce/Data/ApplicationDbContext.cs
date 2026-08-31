using ApiEcommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Data
{
    /// <summary>
    /// Contexto principal de acceso a datos de la aplicación.
    /// 
    /// <para>
    /// Hereda de <see cref="DbContext"/>, que es la clase de Entity Framework Core
    /// encargada de administrar la comunicación entre la aplicación y la base de datos.
    /// </para>
    /// 
    /// <para>
    /// A través de este contexto podemos consultar, agregar, modificar y eliminar
    /// información de las entidades que forman parte de nuestra aplicación.
    /// </para>
    /// </summary>
    public class ApplicationDbContext : DbContext
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
        /// base <see cref="DbContext"/>. De esta forma, Entity Framework Core
        /// recibe la configuración necesaria para trabajar con la base de datos.
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


    }
}
