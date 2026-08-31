using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// DTO utilizado para recibir los datos necesarios para crear un usuario.
    /// </summary>
    public class CreateUserDto
    {
        /// <summary>
        /// Nombre de usuario utilizado para identificar al usuario.
        /// </summary>
        [Required(ErrorMessage = "El campo username es requerido")]
        public string? Username { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        [Required(ErrorMessage = "El campo name es requerido")]
        public string? Name { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        [Required(ErrorMessage = "El campo password es requerido")]
        public string? Password { get; set; }

        /// <summary>
        /// Rol que tendrá el usuario dentro de la aplicación.
        /// </summary>
        [Required(ErrorMessage = "El campo role es requerido")]
        public string? Role { get; set; }
    }
}
