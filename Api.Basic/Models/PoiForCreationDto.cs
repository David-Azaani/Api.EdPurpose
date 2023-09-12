using System.ComponentModel.DataAnnotations;

namespace Api.Basic.Models
{
    public class PoiForCreationDto
    {
        // as in creation we don't need to have id , we made a dto for creation!
        // use separate dto for create update and delete!

        [Required(ErrorMessage = "Blah Blah Blah...")]
        // System.ComponentModel.DataAnnotations. 
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }


        // this approach is not good for validation best is to use fluent validation!
    }
}
