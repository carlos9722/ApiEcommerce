using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// DTO utilizado para recibir las credenciales necesarias para iniciar sesión.
    /// </summary>
    public class UserLoginDto
    {
        /// <summary>
        /// Nombre de usuario utilizado para la autenticación.
        /// </summary>
        [Required(ErrorMessage = "El campo username es requerido")]
        public string? Username { get; set; }

        /// <summary>
        /// Contraseña utilizada para la autenticación.
        /// </summary>
        [Required(ErrorMessage = "El campo password es requerido")]
        public string? Password { get; set; }
    }
}
