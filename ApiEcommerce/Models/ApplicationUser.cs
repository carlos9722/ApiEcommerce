using Microsoft.AspNetCore.Identity;

namespace ApiEcommerce.Models
{
    /// <summary>
    /// Representa un usuario dentro del sistema.
    /// Extiende la funcionalidad proporcionada por ASP.NET Core Identity,
    /// permitiendo gestionar la autenticación, autorización y datos
    /// relacionados con los usuarios de la aplicación.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Nombre del usuario.
        /// Permite almacenar el nombre con el que se identifica
        /// el usuario dentro del sistema.
        /// </summary>
        public string? Name { get; set; }
    }
}
