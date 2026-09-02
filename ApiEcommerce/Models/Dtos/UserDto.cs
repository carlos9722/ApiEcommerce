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
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para iniciar sesión.
        /// </summary>
        public string? Username { get; set; }
    }
}
