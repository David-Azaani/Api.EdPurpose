namespace Api.Basic.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    
    // dto(show to user) vs entity(save in db)
    
}