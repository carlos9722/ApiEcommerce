using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace ApiEcommerce.Repository
{
    /// <summary>
    /// Implementa las operaciones de acceso a datos relacionadas con los productos.
    /// Utiliza ApplicationDbContext para consultar y modificar la información
    /// de productos almacenada en la base de datos.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// Contexto de Entity Framework Core utilizado para acceder
        /// a la base de datos.
        /// </summary>
        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Inicializa una nueva instancia del repositorio de productos.
        /// </summary>
        /// <param name="db">Contexto de base de datos de la aplicación.</param>
        public ProductRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Realiza la compra de una cantidad determinada de un producto.
        /// Reduce el stock disponible y guarda los cambios en la base de datos.
        /// </summary>
        /// <param name="name">Nombre del producto que se desea comprar.</param>
        /// <param name="quantity">Cantidad de unidades que se desean comprar.</param>
        /// <returns>
        /// true si la compra se realizó correctamente;
        /// false si el producto no existe, la cantidad no es válida
        /// o no hay suficiente stock.
        /// </returns>
        public bool BuyProduct(string name, int quantity)
        {
            if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
            {
                return false;
            }

            var product = _db.Products
                .FirstOrDefault(p =>
                    p.Name.ToLower().Trim() == name.ToLower().Trim());

            if (product == null || product.Stock < quantity)
            {
                return false;
            }

            product.Stock -= quantity;

            _db.Products.Update(product);

            return Save();
        }

        /// <summary>
        /// Registra un nuevo producto en la base de datos.
        /// </summary>
        /// <param name="product">Producto que se desea crear.</param>
        /// <returns>
        /// true si el producto fue creado correctamente;
        /// false si el producto es null o no se pudieron guardar los cambios.
        /// </returns>
        public bool CreateProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            product.CreationDate = DateTime.UtcNow;
            product.UpdateDate = null;

            _db.Products.Add(product);

            return Save();
        }

        /// <summary>
        /// Elimina un producto de la base de datos.
        /// </summary>
        /// <param name="product">Producto que se desea eliminar.</param>
        /// <returns>
        /// true si el producto fue eliminado correctamente;
        /// false si el producto es null o no se pudieron guardar los cambios.
        /// </returns>
        public bool DeleteProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            _db.Products.Remove(product);

            return Save();
        }

        /// <summary>
        /// Obtiene un producto mediante su identificador.
        /// Incluye la información de la categoría relacionada.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>
        /// El producto encontrado con su categoría,
        /// o null si no existe.
        /// </returns>
        public Product? GetProduct(int id)
        {
            if (id <= 0)
            {
                return null;
            }

            return _db.Products
                .Include(p => p.Category)
                .FirstOrDefault(p => p.ProductId == id);
        }

        /// <summary>
        /// Obtiene todos los productos registrados.
        /// Los productos se ordenan alfabéticamente por nombre
        /// e incluyen su categoría relacionada.
        /// </summary>
        /// <returns>Una colección con todos los productos.</returns>
        public ICollection<Product> GetProducts()
        {
            return _db.Products
                .Include(p => p.Category)
                .OrderBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Obtiene los productos pertenecientes a una categoría específica.
        /// </summary>
        /// <param name="categoryId">Identificador de la categoría.</param>
        /// <returns>
        /// Una colección de productos pertenecientes a la categoría indicada.
        /// </returns>
        public ICollection<Product> GetProductsForCategory(int categoryId)
        {
            if (categoryId <= 0)
            {
                return new List<Product>();
            }

            return _db.Products
                .Include(p => p.Category)
                .Where(p => p.CategoryId == categoryId)
                .OrderBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Obtiene los productos registrados de forma paginada.
        /// </summary>
        /// <param name="pageNumber">Número de página que se desea obtener.</param>
        /// <param name="pageSize">Cantidad de productos que se incluirán en cada página.</param>
        /// <returns>
        /// Una colección de productos correspondiente a la página solicitada.
        /// </returns>
        public ICollection<Product> GetProductsInPages(int pageNumber, int pageSize)
        {
            return _db.Products.OrderBy(p => p.ProductId)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        }


        /// <summary>
        /// Obtiene los productos registrados de forma paginada.
        /// </summary>
        /// <param name="pageNumber">Número de página que se desea obtener.</param>
        /// <param name="pageSize">Cantidad de productos que se incluirán en cada página.</param>
        /// <returns>
        /// Una colección de productos correspondiente a la página solicitada.
        /// </returns>
        public int GetTotalProducts()
        {
            return _db.Products.Count();
        }


        /// <summary>
        /// Comprueba si existe un producto con el identificador indicado.
        /// </summary>
        /// <param name="id">Identificador del producto.</param>
        /// <returns>true si el producto existe; de lo contrario, false.</returns>
        public bool ProductExists(int id)
        {
            if (id <= 0)
            {
                return false;
            }

            return _db.Products.Any(p => p.ProductId == id);
        }

        /// <summary>
        /// Comprueba si existe un producto con el nombre indicado.
        /// La comparación ignora espacios al inicio y final
        /// y no distingue entre mayúsculas y minúsculas.
        /// </summary>
        /// <param name="name">Nombre del producto.</param>
        /// <returns>true si existe; de lo contrario, false.</returns>
        public bool ProductExists(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return false;
            }

            return _db.Products.Any(p =>
                p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        /// <summary>
        /// Guarda en la base de datos los cambios pendientes realizados
        /// mediante el contexto de Entity Framework Core.
        /// </summary>
        /// <returns>
        /// true si SaveChanges se ejecuta correctamente;
        /// false si no se pudieron guardar los cambios.
        /// </returns>
        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        /// <summary>
        /// Busca productos por nombre o descripción.
        /// La búsqueda ignora mayúsculas, minúsculas y espacios
        /// al inicio y final del término.
        /// </summary>
        /// <param name="searchTerm">Texto que se desea buscar.</param>
        /// <returns>
        /// Una colección de productos que coinciden con el término de búsqueda.
        /// </returns>
        public ICollection<Product> SearchProducts(string searchTerm)
        {
            IQueryable<Product> query = _db.Products;

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchTermLowered = searchTerm.ToLower().Trim();

                query = query
                    .Include(p => p.Category)
                    .Where(p =>
                        p.Name.ToLower().Trim().Contains(searchTermLowered) ||
                        p.Description.ToLower().Trim().Contains(searchTermLowered));
            }

            return query
                .OrderBy(p => p.Name)
                .ToList();
        }

        /// <summary>
        /// Actualiza la información de un producto existente
        /// y registra la fecha de modificación.
        /// </summary>
        /// <param name="product">Producto con la información actualizada.</param>
        /// <returns>
        /// true si el producto fue actualizado correctamente;
        /// false si el producto es null o no se pudieron guardar los cambios.
        /// </returns>
        public bool UpdateProduct(Product product)
        {
            if (product == null)
            {
                return false;
            }

            product.UpdateDate = DateTime.UtcNow;

            _db.Products.Update(product);

            return Save();
        }
    }
}
