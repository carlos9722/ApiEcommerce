namespace ApiEcommerce.Models.Dtos.Responses
{
    /// <summary>
    /// Representa la respuesta de una consulta paginada, permitiendo trabajar
    /// con diferentes tipos de datos mediante el uso de un tipo genérico.
    /// </summary>
    /// <typeparam name="T">
    /// Tipo de dato de los elementos que se incluirán en la colección de resultados.
    /// </typeparam>
    public class PaginationResponse<T>
    {
        /// <summary>
        /// Obtiene o establece el número de la página actual.
        /// </summary>
        public int PageNumber { get; set; }

        /// <summary>
        /// Obtiene o establece la cantidad de elementos que contiene cada página.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Obtiene o establece el número total de páginas disponibles.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Obtiene o establece los elementos correspondientes a la página actual.
        /// El tipo de los elementos está definido por el parámetro genérico T.
        /// </summary>
        public ICollection<T> Items { get; set; } = new List<T>();
    }
}
