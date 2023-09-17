using System.ComponentModel.DataAnnotations;

namespace Api.Basic.Models;

    public class PoiForUpdateDto
    {
        // as in creation we don't need to have id , we made a dto for creation!
        // use separated dto for create update and delete!
        // because id is made by backend and also user doesn't need to set id!

        [Required(ErrorMessage = "Blah Blah Blah...")]
        // System.ComponentModel.DataAnnotations. check this to see what's in it!
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }


        // this approach is not good for validation best is to use fluent validation!
    }

