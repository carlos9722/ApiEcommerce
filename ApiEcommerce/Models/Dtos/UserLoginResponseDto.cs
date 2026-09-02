namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// DTO utilizado para devolver la respuesta del proceso de inicio de sesión.
    /// </summary>
    public class UserLoginResponseDto
    {
        /// <summary>
        /// Información del usuario autenticado.
        /// </summary>
        public UserDataDto? User { get; set; }

        /// <summary>
        /// Token generado para autenticar las siguientes solicitudes.
        /// </summary>
        public string? Token { get; set; }

        /// <summary>
        /// Mensaje asociado al resultado del inicio de sesión.
        /// </summary>
        public string? Message { get; set; }
    }
}