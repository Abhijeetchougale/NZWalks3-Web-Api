using NZWalks3.API.Model.Domain;

namespace NZWalks3.API.Repository
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllAsync();
        Task<Walk> GetAsync(Guid Id);
        Task<Walk> CreateAsync(Walk walk);
        Task<Walk> UpdateAsync(Guid Id, Walk walk);
        Task<Walk> DeleteAsync(Guid Id);
        Task<List<Walk>> FilterAsync(string? filterOn= null, string? SearchText=null);
        Task<List<Walk>> SortAsync(string? sortBy = null, bool? isAssending = true);
        Task<List<Walk>> PaginationAsync(int pageNumber=1 , int pageSize=1000);
        Task<List<Walk>> FiSoPageAsync();



    }
}
