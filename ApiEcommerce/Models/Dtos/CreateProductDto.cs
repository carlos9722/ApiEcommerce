namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Contiene los datos necesarios para crear un nuevo producto.
    /// </summary>
    public class CreateProductDto
    {
        /// <summary>
        /// Nombre comercial del producto.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio de venta del producto.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL de la imagen del producto.
        /// </summary>
        public string? ImgUrl { get; set; }

        /// <summary>
        /// Archivo de imagen que se asociará al producto.
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Código SKU utilizado para identificar el producto.
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad inicial disponible del producto en inventario.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece el producto.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
