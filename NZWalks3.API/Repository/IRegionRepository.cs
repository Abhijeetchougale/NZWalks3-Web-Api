using NZWalks3.API.Model.Domain;

namespace NZWalks3.API.Repository
{
    public interface IRegionRepository
    {
        Task<List<Region>>GetAllAsync();
        Task<Region> GetByIdAsync(Guid id);
        Task<Region> CreateAsync(Region region);
        Task<Region> UpadateAsync(Guid id, Region region);
        Task<Region> DeleteAsync(Guid id);


    }
}
