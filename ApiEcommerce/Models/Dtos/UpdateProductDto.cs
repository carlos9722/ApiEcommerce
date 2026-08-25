namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Contiene los datos que pueden ser modificados
    /// al actualizar un producto existente.
    /// </summary>
    public class UpdateProductDto
    {
        /// <summary>
        /// Nombre comercial del producto.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción actualizada del producto.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio actualizado del producto.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// URL de la imagen del producto.
        /// </summary>
        public string ImgUrl { get; set; } = string.Empty;

        /// <summary>
        /// Código SKU del producto.
        /// </summary>
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad actual disponible en inventario.
        /// </summary>
        public int Stock { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece el producto.
        /// </summary>
        public int CategoryId { get; set; }
    }
}
