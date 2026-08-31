using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace ApiEcommerce.Repository
{
    /// <summary>
    /// Implementa las operaciones de acceso a datos y autenticación de usuarios.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        public readonly ApplicationDbContext _db;

        private string? secretKey;

        /// <summary>
        /// Inicializa el repositorio y obtiene la clave utilizada para generar JWT.
        /// </summary>
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }

        /// <summary>
        /// Busca un usuario por su identificador.
        /// </summary>
        public User? GetUser(int id)
        {
            return _db.Users.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Obtiene todos los usuarios ordenados por username.
        /// </summary>
        public ICollection<User> GetUsers()
        {
            return _db.Users.OrderBy(u => u.Username).ToList();
        }

        /// <summary>
        /// Verifica si el username no está registrado.
        /// </summary>
        public bool IsUniqueUser(string username)
        {
            return !_db.Users.Any(
                u => u.Username.ToLower().Trim() == username.ToLower().Trim());
        }

        /// <summary>
        /// Autentica al usuario y genera un token JWT si las credenciales son válidas.
        /// </summary>
        /// <param name="userLoginDto">Username y password ingresados por el usuario.</param>
        /// <returns>Respuesta con usuario, token y mensaje del resultado.</returns>
        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            if (string.IsNullOrEmpty(userLoginDto.Username))
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "El Username es requerido "
                };
            }

            // Busca el usuario utilizando el username recibido.
            var user = await _db.Users.FirstOrDefaultAsync<User>(
                u => u.Username.ToLower().Trim() ==
                     userLoginDto.Username.ToLower().Trim());

            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Username no encontrado"
                };
            }

            // BCrypt compara el password ingresado contra el hash almacenado.
            if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Credenciales son incorrectas"
                };
            }

            // JWT: genera un token que identifica y autoriza al usuario.
            var handlerToken = new JwtSecurityTokenHandler();

            if (string.IsNullOrWhiteSpace(secretKey))
            {
                throw new InvalidOperationException("SecretKey no esta configurada");
            }

            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claims: información que queda asociada al usuario dentro del token.
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.Username),
                    new Claim(ClaimTypes.Role, user.Role ?? string.Empty),
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = handlerToken.CreateToken(tokenDescriptor);

            return new UserLoginResponseDto()
            {
                Token = handlerToken.WriteToken(token),
                User = new UserRegisterDto()
                {
                    Username = user.Username,
                    Name = user.Name,
                    Role = user.Role,
                    Password = user.Password ?? ""
                },
                Message = "Usuario logueado correctamente"
            };
        }

        /// <summary>
        /// Registra un nuevo usuario almacenando su password de forma segura mediante BCrypt.
        /// </summary>
        /// <param name="createUserDto">Datos necesarios para registrar el usuario.</param>
        /// <returns>Usuario registrado.</returns>
        public async Task<User> Register(CreateUserDto createUserDto)
        {
            // Nunca debemos guardar el password original directamente en la BD.
            var encriptedPassword = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password);

            var user = new User()
            {
                Username = createUserDto.Username ?? "No Username",
                Name = createUserDto.Name,
                Role = createUserDto.Role,
                Password = encriptedPassword
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return user;
        }
    }
}
