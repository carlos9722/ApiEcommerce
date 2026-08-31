using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping
{
    /// <summary>
    /// Configuración de los mapeos entre la entidad User y sus DTOs.
    /// </summary>
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // Permite convertir User ↔ UserDto en ambas direcciones.
            CreateMap<User, UserDto>().ReverseMap();

            // Permite convertir User ↔ CreateUserDto en ambas direcciones.
            CreateMap<User, CreateUserDto>().ReverseMap();

            // Permite convertir User ↔ UserLoginDto en ambas direcciones.
            CreateMap<User, UserLoginDto>().ReverseMap();

            // Permite convertir User ↔ UserLoginResponseDto en ambas direcciones.
            CreateMap<User, UserLoginResponseDto>().ReverseMap();
        }
    }
}
