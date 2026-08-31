namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// DTO utilizado para recibir los datos necesarios para registrar un usuario.
    /// </summary>
    public class UserRegisterDto
    {
        /// <summary>
        /// Identificador del usuario, si ya se encuentra disponible.
        /// </summary>
        public string? ID { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para el registro.
        /// </summary>
        public required string Username { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public required string Password { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Rol asignado al usuario.
        /// </summary>
        public string? Role { get; set; }
    }
}