namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// DTO utilizado para representar la información de un usuario.
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para iniciar sesión.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Contraseña asociada al usuario.
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// Rol asignado al usuario dentro de la aplicación.
        /// </summary>
        public string? Role { get; set; }
    }
}
