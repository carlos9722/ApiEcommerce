using ApiEcommerce.Data;
using ApiEcommerce.Repository.IRepository;

namespace ApiEcommerce.Repository;

/// <summary>
/// Proporciona las operaciones de acceso a datos para la entidad <see cref="Category"/>.
/// </summary>
public class CategoryRepository : ICategoryRepository
{
    // Contexto de Entity Framework Core utilizado para acceder a la base de datos.
    private readonly ApplicationDbContext _db;

    /// <summary>
    /// Inicializa una nueva instancia del repositorio de categorías.
    /// </summary>
    /// <param name="db">
    /// Contexto de base de datos proporcionado mediante inyección de dependencias.
    /// </param>
    public CategoryRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Determina si existe una categoría con el identificador especificado.
    /// </summary>
    /// <param name="id">Identificador único de la categoría.</param>
    /// <returns>
    /// <c>true</c> si la categoría existe; de lo contrario, <c>false</c>.
    /// </returns>
    public bool CategoryExists(int id)
    {
        return _db.Categories.Any(category => category.Id == id);
    }

    /// <summary>
    /// Determina si existe una categoría con el nombre especificado.
    /// </summary>
    /// <param name="name">Nombre de la categoría que se desea buscar.</param>
    /// <returns>
    /// <c>true</c> si existe una categoría con ese nombre; de lo contrario, <c>false</c>.
    /// </returns>
    public bool CategoryExists(string name)
    {
        return _db.Categories.Any(category =>
            category.Name.Trim().ToLower() == name.Trim().ToLower());
    }

    /// <summary>
    /// Crea una nueva categoría y guarda los cambios en la base de datos.
    /// </summary>
    /// <param name="category">Categoría que se desea crear.</param>
    /// <returns>
    /// <c>true</c> si los cambios se guardan correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    public bool CreateCategory(Category category)
    {
        category.CreationDate = DateTime.UtcNow;

        _db.Categories.Add(category);

        return Save();
    }

    /// <summary>
    /// Elimina una categoría y guarda los cambios en la base de datos.
    /// </summary>
    /// <param name="category">Categoría que se desea eliminar.</param>
    /// <returns>
    /// <c>true</c> si los cambios se guardan correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    public bool DeleteCategory(Category category)
    {
        _db.Categories.Remove(category);

        return Save();
    }

    /// <summary>
    /// Obtiene todas las categorías ordenadas alfabéticamente por nombre.
    /// </summary>
    /// <returns>Una colección de categorías ordenadas por nombre.</returns>
    public ICollection<Category> GetCategories()
    {
        return _db.Categories
            .OrderBy(category => category.Name)
            .ToList();
    }

    /// <summary>
    /// Obtiene una categoría a partir de su identificador.
    /// </summary>
    /// <param name="id">Identificador único de la categoría.</param>
    /// <returns>
    /// La categoría encontrada; si no existe una categoría con el identificador especificado,
    /// devuelve <c>null</c>.
    /// </returns>
    public Category? GetCategory(int id)
    {
        return _db.Categories
            .FirstOrDefault(category => category.Id == id);
    }

    /// <summary>
    /// Guarda los cambios pendientes en la base de datos.
    /// </summary>
    /// <returns>
    /// <c>true</c> si la operación se ejecuta correctamente.
    /// </returns>
    public bool Save()
    {
        _db.SaveChanges();

        return true;
    }

    /// <summary>
    /// Actualiza una categoría existente y guarda los cambios.
    /// </summary>
    /// <param name="category">Categoría con la información actualizada.</param>
    /// <returns>
    /// <c>true</c> si los cambios se guardan correctamente; de lo contrario, <c>false</c>.
    /// </returns>
    public bool UpdateCategory(Category category)
    {
        _db.Categories.Update(category);

        return Save();
    }
}
