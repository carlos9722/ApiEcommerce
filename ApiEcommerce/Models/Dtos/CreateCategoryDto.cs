using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Representa los datos necesarios para crear una nueva categoría.
    /// </summary>
    public class CreateCategoryDto
    {
        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        /// <remarks>
        /// El nombre es obligatorio y debe contener entre 3 y 50 caracteres.
        /// </remarks>
        [Required(ErrorMessage = "El Nombre es Obligatorio.")]
        [MaxLength(50, ErrorMessage = "El Nombre No puede tener más de 50 caracteres.")]
        [MinLength(3, ErrorMessage = "El Nombre no puede tener menos de 3 caracteres.")]
        public string Name { get; set; } = string.Empty;
    }
}
