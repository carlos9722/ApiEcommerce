namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Contiene la información de un producto que será enviada al cliente.
    /// </summary>
    public class ProductDto
    {
        /// <summary>
        /// Identificador único del producto.
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre comercial del producto.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción del producto.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio actual del producto.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL de la imagen asociada al producto.
        /// </summary>
        public string? ImgUrl { get; set; }

        /// <summary>
        /// Archivo de imagen asociado al producto.
        /// </summary>
        public IFormFile? Image { get; set; }

        /// <summary>
        /// Código SKU utilizado para identificar el producto.
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad disponible del producto en inventario.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Fecha y hora en la que se creó el producto.
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Fecha y hora de la última actualización del producto.
        /// Puede ser <c>null</c> si el producto aún no ha sido actualizado.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece el producto.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Nombre de la categoría a la que pertenece el producto.
        /// </summary>
        public string CategoryName { get; set; } = string.Empty;
    }
}
