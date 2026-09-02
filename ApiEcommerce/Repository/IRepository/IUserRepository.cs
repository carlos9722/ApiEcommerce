using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;

namespace ApiEcommerce.Repository.IRepository
{
    /// <summary>
    /// Define las operaciones de acceso a datos relacionadas con los usuarios.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Obtiene todos los usuarios registrados.
        /// </summary>
        ICollection<ApplicationUser> GetUsers();

        /// <summary>
        /// Obtiene un usuario por su identificador.
        /// </summary>
        /// <param name="string">Identificador del usuario.</param>
        /// <returns>El usuario encontrado o null si no existe.</returns>
        ApplicationUser? GetUser(string id);

        /// <summary>
        /// Verifica si un nombre de usuario está disponible.
        /// </summary>
        /// <param name="username">Nombre de usuario a validar.</param>
        /// <returns>True si el usuario es único; de lo contrario, false.</returns>
        bool IsUniqueUser(string username);

        /// <summary>
        /// Autentica un usuario utilizando sus credenciales.
        /// </summary>
        /// <param name="userLoginDto">Credenciales del usuario.</param>
        /// <returns>Información del usuario autenticado y su token.</returns>
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);

        /// <summary>
        /// Registra un nuevo usuario.
        /// </summary>
        /// <param name="createUserDto">Datos necesarios para crear el usuario.</param>
        /// <returns>Usuario creado.</returns>
        Task<UserDataDto> Register(CreateUserDto createUserDto);
    }
}
