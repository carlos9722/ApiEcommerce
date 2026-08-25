using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using AutoMapper;

namespace ApiEcommerce.Mapping
{
    /// <summary>
    /// Configuración de AutoMapper para las conversiones
    /// relacionadas con la entidad Product y sus DTOs.
    /// </summary>
    public class ProductProfile : Profile
    {
        /// <summary>
        /// Configura los mapeos entre Product y los DTOs
        /// utilizados para consultar, crear y actualizar productos.
        /// </summary>
        public ProductProfile()
        {
            // Mapeo entre Product y ProductDto.
            // CategoryName obtiene su valor desde Category.Name.
            CreateMap<Product, ProductDto>()
                .ForMember(
                    dest => dest.CategoryName,
                    opt => opt.MapFrom(src => src.Category.Name)
                )
                .ReverseMap();

            // Mapeo utilizado para crear productos.
            CreateMap<CreateProductDto, Product>();

            // Mapeo utilizado para actualizar productos.
            CreateMap<UpdateProductDto, Product>();
        }
    }
}
