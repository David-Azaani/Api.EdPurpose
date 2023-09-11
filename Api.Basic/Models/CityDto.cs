namespace Api.Basic.Models;

public class CityDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
   
    // dto(show to user) vs entity(save in db)

    public int NumberOfPoi => PoiCollection.Count; // get !

    public ICollection<PoiDto> PoiCollection { get; set; } = new List<PoiDto>();
    // always set collection to empty one to avoid getting null reference error!

}