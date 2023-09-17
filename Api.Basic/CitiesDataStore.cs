using Api.Basic.Models;

namespace Api.Basic
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
       //public static CitiesDataStore Current { get; } = new CitiesDataStore();
       // if you want to comment this out you have to use static instance instead of injection on all controller
        // and comment registering this on program cs

        public CitiesDataStore()
        {
            // init dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                     Id = 1,
                     Name = "City 1",
                     Description = "The one with that big park.",
                     PoiCollection = new List<PoiDto>()
                     {
                         new PoiDto() {
                             Id = 1,
                             Name = "Central Park",
                             Description = "The most visited urban park in the United States." },
                          new PoiDto() {
                             Id = 2,
                             Name = "Empire State Building",
                             Description = "A 102-story skyscraper located in Midtown Manhattan." },
                     }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "City 2",
                    Description = "The one with the cathedral that was never really finished.",
                    PoiCollection = new List<PoiDto>()
                     {
                         new PoiDto() {
                             Id = 3,
                             Name = "Cathedral of Our Lady",
                             Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans." },
                          new PoiDto() {
                             Id = 4,
                             Name = "Antwerp Central Station",
                             Description = "The the finest example of railway architecture in Belgium." },
                     }
                },
                new CityDto()
                {
                    Id= 3,
                    Name = "City 3",
                    Description = "The one with that big tower.",
                    PoiCollection = new List<PoiDto>()
                     {
                         new PoiDto() {
                             Id = 5,
                             Name = "Eiffel Tower",
                             Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel." },
                          new PoiDto() {
                             Id = 6,
                             Name = "The Louvre",
                             Description = "The world's largest museum." },
                     }
                }
            };

        }

    }
}
