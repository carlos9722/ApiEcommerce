using ApiEcommerce.Models;

namespace ApiEcommerce.Repository.IRepository
{
    /// <summary>
    /// Define las operaciones de acceso a datos relacionadas con los productos.
    /// </summary>
    public interface IProductRepository
    {
        /// <summary>
        /// Obtiene todos los productos registrados.
        /// </summary>
        /// <returns>Una colección de productos.</returns>
        ICollection<Product> GetProducts();

        /// <summary>
        /// Obtiene todos los productos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="categoryId">Identificador de la categoría.</param>
        /// <returns>Una colección de productos de la categoría indicada.</returns>
        ICollection<Product> GetProductsForCategory(int categoryId);

        /// <summary>
        /// Busca productos cuyo nombre coincida con el término de búsqueda.
        /// </summary>
        /// <param name="searchTerm">Texto utilizado para realizar la búsqueda.</param>
        /// <returns>Una colección de productos que coinciden con la búsqueda.</returns>
        ICollection<Product> SearchProducts(string searchTerm);

        /// <summary>
        /// Obtiene un producto mediante su identificador.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>El producto encontrado o null si no existe.</returns>
        Product? GetProduct(int id);

        /// <summary>
        /// Realiza una compra reduciendo la cantidad disponible
        /// del producto indicado.
        /// </summary>
        /// <param name="name">Nombre del producto.</param>
        /// <param name="quantity">Cantidad que se desea comprar.</param>
        /// <returns>
        /// true si la compra se realizó correctamente;
        /// false si no fue posible realizarla.
        /// </returns>
        bool BuyProduct(string name, int quantity);

        /// <summary>
        /// Comprueba si existe un producto con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>true si existe; de lo contrario, false.</returns>
        bool ProductExists(int id);

        /// <summary>
        /// Comprueba si existe un producto con el nombre indicado.
        /// </summary>
        /// <param name="name">Nombre del producto.</param>
        /// <returns>true si existe; de lo contrario, false.</returns>
        bool ProductExists(string name);

        /// <summary>
        /// Registra un nuevo producto.
        /// </summary>
        /// <param name="product">Producto que se desea registrar.</param>
        /// <returns>true si la operación fue exitosa; de lo contrario, false.</returns>
        bool CreateProduct(Product product);

        /// <summary>
        /// Actualiza la información de un producto existente.
        /// </summary>
        /// <param name="product">Producto con la información actualizada.</param>
        /// <returns>true si la operación fue exitosa; de lo contrario, false.</returns>
        bool UpdateProduct(Product product);

        /// <summary>
        /// Elimina un producto existente.
        /// </summary>
        /// <param name="product">Producto que se desea eliminar.</param>
        /// <returns>true si la operación fue exitosa; de lo contrario, false.</returns>
        bool DeleteProduct(Product product);

        /// <summary>
        /// Guarda los cambios pendientes en la base de datos.
        /// </summary>
        /// <returns>true si los cambios se guardaron correctamente.</returns>
        bool Save();
    }
}
