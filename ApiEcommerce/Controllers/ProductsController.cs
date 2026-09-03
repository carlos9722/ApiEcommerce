using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;
using ApiEcommerce.Repository.IRepository;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiEcommerce.Controllers
{
    /// <summary>
    /// Controlador encargado de gestionar las operaciones relacionadas con productos.
    /// </summary>
    [Authorize(Roles = "Admin")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersionNeutral]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtiene todos los productos registrados.
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProducts()
        {
            // Se consulta el repositorio y se transforman las entidades a DTOs.
            var products = _productRepository.GetProducts();
            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return Ok(productsDto);
        }

        /// <summary>
        /// Obtiene un producto utilizando su identificador.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("{productId:int}", Name = "GetProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProduct(int productId)
        {
            var product = _productRepository.GetProduct(productId);

            // Si el repositorio no encuentra el producto, retornamos 404.
            if (product == null)
            {
                return NotFound($"El producto con el id {productId} no existe");
            }

            var productDto = _mapper.Map<ProductDto>(product);

            return Ok(productDto);
        }

        /// <summary>
        /// Crea un nuevo producto en el catálogo.
        /// Valida que el producto y la categoría no existan previamente
        /// y procesa la imagen asociada, si se proporciona.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CreateProduct([FromForm] CreateProductDto createProductDto)
        {
            if (createProductDto == null)
            {
                return BadRequest(ModelState);
            }

            // Validamos que no exista otro producto con el mismo nombre.
            if (_productRepository.ProductExists(createProductDto.Name))
            {
                ModelState.AddModelError("CustomError", "El producto ya existe");
                return BadRequest(ModelState);
            }

            // Antes de crear el producto verificamos que la categoría exista.
            if (!_categoryRepository.CategoryExists(createProductDto.CategoryId))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"La categoría con el {createProductDto.CategoryId} no existe");

                return BadRequest(ModelState);
            }

            // Convertimos el DTO recibido desde la API en una entidad Product.
            var product = _mapper.Map<Product>(createProductDto);

            // Procesamos la imagen del producto cuando se proporciona.
            if (createProductDto.Image != null)
            {
                UploadProductImage(createProductDto, product);
            }
            else
            {
                // Asignamos una imagen predeterminada cuando no se proporciona una imagen.
                product.ImgUrl = "https://placehold.co/300x300";
            }

            // Guardamos el producto en la base de datos.
            if (!_productRepository.CreateProduct(product))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al guardar el registro {product.Name}");

                return StatusCode(500, ModelState);
            }

            // Recuperamos el producto creado para devolver la información completa.
            var createdProduct = _productRepository.GetProduct(product.ProductId);

            // Convertimos la entidad creada al DTO que será enviado al cliente.
            var productoDto = _mapper.Map<ProductDto>(createdProduct);

            // Retornamos el producto creado y la ruta para consultarlo.
            return CreatedAtRoute(
                "GetProduct",
                new { productId = product.ProductId },
                productoDto);
        }

        /// <summary>
        /// Obtiene todos los productos asociados a una categoría.
        /// </summary>
        [HttpGet( "searchProductByCategory/{categoryId:int}", Name = "GetProductsForCategory")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult GetProductsForCategory(int categoryId)
        {
            var products = _productRepository.GetProductsForCategory(categoryId);

            if (products.Count == 0)
            {
                return NotFound(
                    $"Los productos con la categoría {categoryId} no existen");
            }

            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return Ok(productsDto);
        }

        /// <summary>
        /// Busca productos por nombre o descripción.
        /// </summary>
        [HttpGet("searchProductByNameDescription/{searchTerm}", Name = "SearchProducts")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult SearchProducts(string searchTerm)
        {
            var products = _productRepository.SearchProducts(searchTerm);

            if (products.Count == 0)
            {
                return NotFound(
                    $"Los productos con el nombre o descripción '{searchTerm}' no existen");
            }

            var productsDto = _mapper.Map<List<ProductDto>>(products);

            return Ok(productsDto);
        }

        /// <summary>
        /// Realiza la compra de una cantidad determinada de un producto.
        /// </summary>
        [HttpPatch("buyProduct/{name}/{quantity:int}", Name = "BuyProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult BuyProduct(string name, int quantity)
        {
            // Validamos los datos mínimos antes de consultar el repositorio.
            if (string.IsNullOrWhiteSpace(name) || quantity <= 0)
            {
                return BadRequest(
                    "El nombre del producto o la cantidad no son válidos");
            }

            var foundProduct = _productRepository.ProductExists(name);

            if (!foundProduct)
            {
                return NotFound($"El producto con el nombre {name} no existe");
            }

            // El repositorio se encarga de realizar la compra y validar el stock.
            if (!_productRepository.BuyProduct(name, quantity))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"No se pudo comprar el producto {name} o la cantidad solicitada es mayor al stock disponible");

                return BadRequest(ModelState);
            }

            var units = quantity == 1 ? "unidad" : "unidades";

            return Ok($"Se compro {quantity} {units} del producto '{name}'");
        }

        /// <summary>
        /// Actualiza la información de un producto existente.
        /// </summary>
        [HttpPut("{productId:int}", Name = "UpdateProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateProduct(
            int productId,
            [FromForm] UpdateProductDto updateProductDto)
        {
            if (updateProductDto == null)
            {
                return BadRequest(ModelState);
            }

            if (!_productRepository.ProductExists(productId))
            {
                ModelState.AddModelError("CustomError", "El producto no existe");
                return BadRequest(ModelState);
            }

            // La categoría también debe existir antes de actualizar el producto.
            if (!_categoryRepository.CategoryExists(updateProductDto.CategoryId))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"La categoría con el {updateProductDto.CategoryId} no existe");

                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(updateProductDto);

            // El ID utilizado para actualizar corresponde al recibido en la URL.
            product.ProductId = productId;

            // Agregando imagen
            if (updateProductDto.Image != null)
            {
                UploadProductImage(updateProductDto, product);
            }
            else
            {
                product.ImgUrl = "https://placehold.co/300x300";
            }

            if (!_productRepository.UpdateProduct(product))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al actualizar el registro {product.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }


        /// <summary>
        /// Carga la imagen del producto en el directorio de almacenamiento
        /// y actualiza las rutas de acceso a la imagen en la entidad Product.
        /// </summary>
        /// <param name="productDto">
        /// DTO que contiene el archivo de imagen que será almacenado.
        /// </param>
        /// <param name="product">
        /// Entidad Product a la que se asociará la imagen almacenada.
        /// </param>
        private void UploadProductImage(dynamic productDto, Product product)
        {
            // Generamos un nombre único para evitar conflictos entre archivos.
            string fileName =
                product.ProductId +
                Guid.NewGuid().ToString() +
                Path.GetExtension(productDto.Image.FileName);

            // Definimos el directorio donde se almacenarán las imágenes.
            var imagesFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "ProductsImages");

            // Creamos el directorio si todavía no existe.
            if (!Directory.Exists(imagesFolder))
            {
                Directory.CreateDirectory(imagesFolder);
            }

            // Construimos la ruta completa donde se almacenará la imagen.
            var filePath = Path.Combine(imagesFolder, fileName);

            FileInfo file = new FileInfo(filePath);

            // Eliminamos el archivo si ya existe.
            if (file.Exists)
            {
                file.Delete();
            }

            // Creamos el archivo y copiamos la imagen recibida al sistema de archivos.
            using var fileStream = new FileStream(filePath, FileMode.Create);
            productDto.Image.CopyTo(fileStream);

            // Construimos la URL pública utilizada para acceder a la imagen.
            var baseUrl =
                $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

            product.ImgUrl = $"{baseUrl}/ProductsImages/{fileName}";

            // Guardamos la ruta física del archivo en el servidor.
            product.ImgUrlLocal = filePath;
        }


        /// <summary>
        /// Elimina un producto utilizando su identificador.
        /// </summary>
        [HttpDelete("{productId:int}", Name = "DeleteProduct")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeleteProduct(int productId)
        {
            if (productId == 0)
            {
                return BadRequest(ModelState);
            }

            var product = _productRepository.GetProduct(productId);

            if (product == null)
            {
                return NotFound($"El producto con el id {productId} no existe");
            }

            if (!_productRepository.DeleteProduct(product))
            {
                ModelState.AddModelError(
                    "CustomError",
                    $"Algo salió mal al eliminar el registro {product.Name}");

                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
