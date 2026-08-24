using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping
{
    /// <summary>
    /// Configura los mapeos entre la entidad <see cref="Category"/>
    /// y los DTO utilizados por la API.
    /// </summary>
    public class CategoryProfile : Profile
    {
        /// <summary>
        /// Inicializa la configuración de los mapeos de categorías.
        /// </summary>
        public CategoryProfile()
        {
            // Permite convertir una Category en CategoryDto y viceversa.
            CreateMap<Category, CategoryDto>().ReverseMap();

            // Permite convertir una Category en CreateCategoryDto y viceversa.
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}
