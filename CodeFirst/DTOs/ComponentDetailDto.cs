namespace CodeFirst.DTOs;

public class ComponentDetailDto
{
    public string Code { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public ManufacturerDto Manufacturer { get; set; } = null!;
    public TypeDto Type { get; set; } = null!;
}