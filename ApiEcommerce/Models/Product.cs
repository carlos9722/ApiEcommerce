using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEcommerce.Models
{
    /// <summary>
    /// Representa un producto del catálogo de la tienda.
    /// Contiene la información necesaria para identificar, describir,
    /// valorar y controlar el inventario del producto.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Identificador único del producto.
        /// Se utiliza como clave primaria en la tabla de productos.
        /// </summary>
        [Key]
        public int ProductId { get; set; }

        /// <summary>
        /// Nombre comercial que identifica al producto dentro del catálogo.
        /// Es un campo obligatorio.
        /// </summary>
        [Required]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Descripción detallada del producto.
        /// Este campo es opcional.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Precio de venta actual del producto.
        /// No permite valores negativos.
        /// </summary>
        [Range(0, double.MaxValue)]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        /// <summary>
        /// Dirección URL de la imagen utilizada para representar
        /// visualmente el producto.
        /// </summary>
        public string ImgUrl { get; set; } = string.Empty;

        /// <summary>
        /// Stock Keeping Unit (SKU).
        /// Código utilizado para identificar de forma única
        /// una referencia del producto dentro del inventario.
        /// Ejemplo: PROD-001-BLK-M.
        /// Es un campo obligatorio.
        /// </summary>
        [Required]
        public string SKU { get; set; } = string.Empty;

        /// <summary>
        /// Cantidad de unidades disponibles del producto en inventario.
        /// No permite valores negativos.
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Stock { get; set; }

        /// <summary>
        /// Fecha y hora en la que se creó el registro del producto.
        /// Se establece automáticamente al crear una nueva instancia.
        /// </summary>
        public DateTime CreationDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Fecha y hora de la última modificación del producto.
        /// Es nullable porque un producto recién creado puede no haber
        /// sido actualizado todavía.
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Identificador de la categoría a la que pertenece el producto.
        /// Actúa como clave foránea (FK) hacia la entidad Category.
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Propiedad de navegación que permite acceder a la categoría
        /// asociada al producto.
        /// 
        /// El atributo ForeignKey indica a Entity Framework Core
        /// que CategoryId es la clave foránea utilizada para establecer
        /// esta relación.
        /// </summary>
        [ForeignKey("CategoryId")]
        public required Category Category { get; set; }
    }
}
