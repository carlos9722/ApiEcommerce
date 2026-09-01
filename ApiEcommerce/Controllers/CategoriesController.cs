using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar las operaciones HTTP relacionadas
    /// con las categorías.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : ControllerBase
    {
        // Repositorio utilizado para consultar y modificar las categorías.
        private readonly ICategoryRepository _categoryRepository;

        // Mapper utilizado para convertir entidades Category en DTOs y viceversa.
        private readonly IMapper _mapper;

        /// <summary>
        /// Inicializa el controlador de categorías.
        /// </summary>
        /// <param name="categoryRepository">
        /// Repositorio utilizado para acceder a los datos de las categorías.
        /// </param>
        /// <param name="mapper">
        /// Servicio de AutoMapper utilizado para realizar conversiones entre entidades y DTOs.
        /// </param>
        public CategoriesController(
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todas las categorías registradas.
        /// </summary>
        /// <returns>
        /// Una respuesta HTTP 200 con la lista de categorías.
        /// </returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();
            var categoriesDto = new List<CategoryDto>();

            foreach (var category in categories)
            {
                categoriesDto.Add(_mapper.Map<CategoryDto>(category));
            }

            return Ok(categoriesDto);
        }

        /// <summary>
        /// Obtiene una categoría específica mediante su identificador.
        /// </summary>
        /// <param name="id">Identificador de la categoría que se desea consultar.</param>
        /// <returns>
        /// Una respuesta HTTP 200 con la categoría encontrada,
        /// o HTTP 404 si la categoría no existe.
        /// </returns>
        [AllowAnonymous]
        [HttpGet("{id:int}", Name = "GetCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetCategory(int id)
        {
            var category = _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return NotFound($"La categoría con el id {id} no existe");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

        /// <summary>
        /// Crea una nueva categoría.
        /// </summary>
        /// <param name="createCategoryDto">
        /// Datos necesarios para crear la categoría.
        /// </param>
        /// <returns>
        /// Una respuesta HTTP 201 si la categoría fue creada correctamente.
        /// </returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateCategory(
            [FromBody] CreateCategoryDto createCategoryDto)
        {
            if (createCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryExists(createCategoryDto.Name))
            {
                ModelState.AddModelError(
                    "CustomError",
                    "La categoría ya existe");

                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(createCategoryDto);

            if (!_categoryRepository.CreateCategory(category))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al guardar el registro {category.Name}");

                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute(
                "GetCategory",
                new { id = category.Id },
                category);
        }

        /// <summary>
        /// Actualiza una categoría existente.
        /// </summary>
        /// <param name="id">
        /// Identificador de la categoría que se desea actualizar.
        /// </param>
        /// <param name="updateCategoryDto">
        /// Datos actualizados de la categoría.
        /// </param>
        /// <returns>
        /// Una respuesta HTTP 204 si la actualización se realiza correctamente,
        /// o un código de error si la categoría no existe o la operación falla.
        /// </returns>
        [HttpPatch("{id:int}", Name = "UpdateCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateCategory(
            int id,
            [FromBody] CreateCategoryDto updateCategoryDto)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La categoría con el id {id} no existe");
            }

            if (updateCategoryDto == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoryRepository.CategoryExists(updateCategoryDto.Name))
            {
                ModelState.AddModelError(
                    "CustomError",
                    "La categoría ya existe");

                return BadRequest(ModelState);
            }

            var category = _mapper.Map<Category>(updateCategoryDto);
            category.Id = id;

            if (!_categoryRepository.UpdateCategory(category))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al actualizar el registro {category.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina una categoría existente mediante su identificador.
        /// </summary>
        /// <param name="id">
        /// Identificador de la categoría que se desea eliminar.
        /// </param>
        /// <returns>
        /// Una respuesta HTTP 204 si la categoría fue eliminada correctamente,
        /// o un código de error si la categoría no existe o la operación falla.
        /// </returns>
        [HttpDelete("{id:int}", Name = "DeleteCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult DeleteCategory(int id)
        {
            if (!_categoryRepository.CategoryExists(id))
            {
                return NotFound($"La categoría con el id {id} no existe");
            }

            var category = _categoryRepository.GetCategory(id);

            if (category == null)
            {
                return NotFound($"La categoría con el id {id} no existe");
            }

            if (!_categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al eliminar el registro {category.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
