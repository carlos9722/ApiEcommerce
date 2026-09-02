using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiEcommerce.Data;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
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


        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa el repositorio y obtiene la clave utilizada para generar JWT.
        /// </summary>
        public UserRepository(ApplicationDbContext db, IConfiguration configuration,
                              UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Busca un usuario por su identificador.
        /// </summary>
        public ApplicationUser? GetUser(string id)
        {
            return _db.ApplicationUsers.FirstOrDefault(u => u.Id == id);
        }

        /// <summary>
        /// Obtiene todos los usuarios ordenados por username.
        /// </summary>
        public ICollection<ApplicationUser> GetUsers()
        {
            return _db.ApplicationUsers.OrderBy(u => u.UserName).ToList();
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
            var user = await _db.ApplicationUsers.FirstOrDefaultAsync<ApplicationUser>(u => u.UserName != null && u.UserName.ToLower().Trim() == userLoginDto.Username.ToLower().Trim());

            if (user == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Username no encontrado"
                };
            }

            if (userLoginDto.Password == null)
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Password requerido"
                };
            }

            // BCrypt compara el password ingresado contra el hash almacenado.
            /*if (!BCrypt.Net.BCrypt.Verify(userLoginDto.Password, user.Password))
            {
                return new UserLoginResponseDto()
                {
                    Token = "",
                    User = null,
                    Message = "Credenciales son incorrectas"
                };
            }*/

            bool isValid = await _userManager.CheckPasswordAsync(user, userLoginDto.Password);
            if (!isValid)
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

            var roles = await _userManager.GetRolesAsync(user);
            var key = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                // Claims: información que queda asociada al usuario dentro del token.
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username",user.UserName ?? string.Empty),
                    new Claim(ClaimTypes.Role, roles.FirstOrDefault() ?? string.Empty),
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
                User = _mapper.Map<UserDataDto>(user),
                Message = "Usuario logueado correctamente"
            };
        }

        /// <summary>
        /// Registra un nuevo usuario almacenando su password de forma segura mediante BCrypt.
        /// </summary>
        /// <param name="createUserDto">Datos necesarios para registrar el usuario.</param>
        /// <returns>Usuario registrado.</returns>
        public async Task<UserDataDto> Register(CreateUserDto createUserDto)
        {
           if (string.IsNullOrEmpty(createUserDto.Username))
            {
                throw new ArgumentNullException("El Username es requerido");
            }

            if (createUserDto.Password == null)
            {
                throw new ArgumentNullException("El Password es requerido");
            }

            var user = new ApplicationUser()
            {
                UserName = createUserDto.Username,
                Email = createUserDto.Username,
                NormalizedEmail = createUserDto.Username.ToUpper(),
                Name = createUserDto.Name
            };

            // Aquí ASP.NET Core Identity hashea la contraseña y guarda el resultado en PasswordHash
            var result = await _userManager.CreateAsync(user, createUserDto.Password);

            if (result.Succeeded)
            {
                var userRole = createUserDto.Role ?? "User";
                var roleExists = await _roleManager.RoleExistsAsync(userRole);
                if (!roleExists)
                {
                    var identityRole = new IdentityRole(userRole);
                    await _roleManager.CreateAsync(identityRole);
                }

                await _userManager.AddToRoleAsync(user, userRole);
                var createdUser = _db.ApplicationUsers.FirstOrDefault(u => u.UserName == createUserDto.Username);
                return _mapper.Map<UserDataDto>(createdUser);
            }

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new ApplicationException($"No se pudo realizar el registro: {errors}");
        }
    }
}
