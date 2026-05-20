using System.ComponentModel.DataAnnotations;

namespace CodeFirst.Models;

public class ComponentType
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(30)]
    public string Abbreviation { get; set; } = null!;

    [Required, MaxLength(150)]
    public string Name { get; set; } = null!;

    public ICollection<Component> Components { get; set; } = new List<Component>();
}