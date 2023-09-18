using Api.Basic.DbContexts;
using Api.Basic.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Basic.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext _context;

        public CityInfoRepository(CityInfoContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await _context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<bool> CityNameMatchesCityId(string? cityName, int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId && c.Name == cityName);
        }

        public async Task<(IEnumerable<City>, PaginationMetadata)> GetCitiesAsync(
            string? name, string? searchQuery, int pageNumber, int pageSize)
        {
            // collection to start from
            var collection = _context.Cities as IQueryable<City>;

            if (!string.IsNullOrWhiteSpace(name))
            {
                name = name.Trim();
                collection = collection.Where(c => c.Name == name);
            }

            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim();
                collection = collection.Where(a => a.Name.Contains(searchQuery)
                    || (a.Description != null && a.Description.Contains(searchQuery)));
            }

            var totalItemCount = await collection.CountAsync();

            var paginationMetadata = new PaginationMetadata(
                totalItemCount, pageSize, pageNumber);

            var collectionToReturn = await collection.OrderBy(c => c.Name)
                .Skip(pageSize * (pageNumber - 1))
                .Take(pageSize)
                .ToListAsync();

            return (collectionToReturn, paginationMetadata);
        }



        public async Task<City?> GetCityAsync(int cityId, bool includePoi)
        {
            if (includePoi)
            {
                return await _context.Cities.Include(c => c.PoiCollection)
                    .Where(c => c.Id == cityId).FirstOrDefaultAsync();

               
            }

            return await _context.Cities
                  .Where(c => c.Id == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> CityExistsAsync(int cityId)
        {
            return await _context.Cities.AnyAsync(c => c.Id == cityId);
        }

        public async Task<Poi?> GetPoiForCityAsync(
            int cityId, 
            int poi)
        {
            return await _context.Poi
               .Where(p => p.CityId == cityId && p.Id == poi)
               .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Poi>> GetPoisForCityAsync(
            int cityId)
        {
            return await _context.Poi
                           .Where(p => p.CityId == cityId).ToListAsync();
        }

        public async Task AddPoiForCityAsync(int cityId, 
            Poi poi)
        {
            var city = await GetCityAsync(cityId, false);
            if (city != null)
            {
                city.PoiCollection.Add(poi); // we doesn't use add async because this operation happens in memory  and not in the IODatabase!
            }
        }

        public void DeletePoi(Poi poi)
        {
            // because it's in memory no not to async!
            _context.Poi.Remove(poi);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}
