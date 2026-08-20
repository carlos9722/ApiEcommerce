using System.ComponentModel.DataAnnotations;

/// <summary>
/// Representa una categoría dentro del sistema.
/// Una categoría permite agrupar o clasificar información relacionada.
/// Por ejemplo: "Electrónica", "Ropa" o "Libros".
/// </summary>
public class Category
{
    /// <summary>
    /// Identificador único de la categoría.
    /// Se utiliza como clave primaria para identificar cada registro
    /// de forma única dentro de la base de datos.
    /// </summary>
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// Nombre que identifica la categoría.
    /// Este campo es obligatorio y no debe quedar vacío al registrar
    /// una nueva categoría.
    /// </summary>
    [Required]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Fecha y hora en la que se creó la categoría.
    /// Permite conocer cuándo fue registrada la categoría en el sistema.
    /// </summary>
    [Required]
    public DateTime CreationDate { get; set; }
}
