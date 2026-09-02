namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Representa los datos de un usuario que se transfieren entre la API
    /// y el cliente mediante un objeto de transferencia de datos (DTO).
    /// 
    /// <para>
    /// Este DTO permite exponer únicamente la información necesaria del usuario,
    /// evitando transferir directamente la entidad completa de usuario.
    /// </para>
    /// </summary>
    public class UserDataDto
    {
        /// <summary>
        /// Identificador único del usuario.
        /// Permite identificar de forma única al usuario dentro del sistema.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Nombre de usuario utilizado para identificar al usuario
        /// dentro del sistema y realizar operaciones de autenticación.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Nombre del usuario.
        /// Permite mostrar el nombre con el que se identifica
        /// el usuario dentro de la aplicación.
        /// </summary>
        public string? Name { get; set; }
    }
}
