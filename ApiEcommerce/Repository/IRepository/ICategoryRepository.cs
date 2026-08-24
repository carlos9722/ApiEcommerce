namespace ApiEcommerce.Repository.IRepository
{
    /// <summary>
    /// Define las operaciones disponibles para consultar y administrar
    /// la entidad Category.
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Obtiene todas las categorías registradas.
        /// <para>
        /// Category es la entidad que representa una categoría en el sistema.
        /// ICollection&lt;Category&gt; representa una colección de objetos Category,
        /// es decir, varias categorías.
        /// </para>
        /// <para>
        /// No es necesario importar System.Collections.Generic porque el proyecto
        /// tiene habilitado ImplicitUsings en el archivo .csproj.
        /// </para>
        /// </summary>
        /// <returns>Una colección de entidades Category.</returns>
        ICollection<Category> GetCategories();

        /// <summary>
        /// Obtiene una categoría específica mediante su identificador.
        /// </summary>
        /// <param name="id">Identificador de la categoría.</param>
        /// <returns>
        /// La entidad Category encontrada; si no existe una categoría con el identificador indicado,
        /// devuelve null.
        /// </returns>
        Category? GetCategory(int id);

        /// <summary>
        /// Verifica si existe una categoría con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador de la categoría.</param>
        /// <returns>True si existe; de lo contrario, false.</returns>
        bool CategoryExists(int id);

        /// <summary>
        /// Verifica si existe una categoría con el nombre indicado.
        /// </summary>
        /// <param name="name">Nombre de la categoría.</param>
        /// <returns>True si existe; de lo contrario, false.</returns>
        bool CategoryExists(string name);

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="category">Entidad Category que se desea crear.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        bool CreateCategory(Category category);

        /// <summary>
        /// Actualiza una categoría existente.
        /// </summary>
        /// <param name="category">Entidad Category con los datos actualizados.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        bool UpdateCategory(Category category);

        /// <summary>
        /// Elimina una categoría existente.
        /// </summary>
        /// <param name="category">Entidad Category que se desea eliminar.</param>
        /// <returns>True si la operación fue exitosa.</returns>
        bool DeleteCategory(Category category);

        /// <summary>
        /// Guarda en la base de datos los cambios realizados.
        /// </summary>
        /// <returns>True si los cambios fueron guardados correctamente.</returns>
        bool Save();
    }
}
