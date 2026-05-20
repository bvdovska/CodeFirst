namespace CodeFirst.DTOs;

public class PCComponentDetailDto
{
    public int Amount { get; set; }
    public ComponentDetailDto Component { get; set; } = null!;
}