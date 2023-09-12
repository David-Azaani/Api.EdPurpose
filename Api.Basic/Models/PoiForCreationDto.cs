namespace Api.Basic.Models
{
    public class PoiForCreationDto
    {
        // as in creation we don't need to have id , we made a dto for creation!
        // use separate dto for create update and delete!
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
