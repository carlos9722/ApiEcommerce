namespace ApiEcommerce.Models.Dtos
{
    /// <summary>
    /// Representa los datos de una categoría que serán enviados o recibidos por la API.
    /// </summary>
    public class CategoryDto
    {
        /// <summary>
        /// Identificador único de la categoría.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Nombre de la categoría.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Fecha y hora en que se creó la categoría.
        /// </summary>
        public DateTime CreationDate { get; set; }
    }
}
