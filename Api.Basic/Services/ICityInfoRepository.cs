using Api.Basic.Entities;

namespace Api.Basic.Services
{
    public interface ICityInfoRepository
    {
        Task<IEnumerable<City>> GetCitiesAsync();
           // tuple
        Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize);


        Task<IEnumerable<City>> GetCitiesAsyncFiletered(string? name);
        Task<IEnumerable<City>> GetCitiesAsyncSearched(string? searchQuery);
        Task<City?> GetCityAsync(int cityId, bool includePoi);
        Task<bool> CityExistsAsync(int cityId);
        Task<IEnumerable<Poi>> GetPoisForCityAsync(int cityId);
        Task<Poi?> GetPoiForCityAsync(int cityId, 
            int pointOfInterestId);
        Task AddPoiForCityAsync(int cityId, Poi poi);
        void DeletePoi(Poi poi); // it's not async because it's not IO operation
        Task<bool> CityNameMatchesCityId(string? cityName, int cityId);
        Task<bool> SaveChangesAsync();
    }
}

// note-9


