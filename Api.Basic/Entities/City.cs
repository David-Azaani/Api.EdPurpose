using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Basic.Entities;

public class City
{
    public City(string name)
    {
        Name = name;
    }
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; }
    //public string Name { get; set; } =string.Empty; or put this on ctor! means name always should be existed!


    [MaxLength(200)]
    public string? Description { get; set; }

    public ICollection<Poi> PointsOfInterest { get; set; }
           = new List<Poi>();


}
// we don't need to use this attribute on cityDto because we use it for get data and not to store!

