using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models
{
    /// <summary>
    /// Representa un usuario almacenado en la aplicación.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para autenticarse.
        /// </summary>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Rol asignado al usuario dentro de la aplicación.
        /// </summary>
        public string? Role { get; set; }
    }
}
