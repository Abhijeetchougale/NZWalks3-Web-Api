using Microsoft.EntityFrameworkCore;
using NZWalks3.API.DatabaseConnection;
using NZWalks3.API.Model.Domain;

namespace NZWalks3.API.Repository
{
    public class SQLConnectionWalk : IWalkRepository
    {
        private readonly NZWalkDBContext dbContext;

        public SQLConnectionWalk(NZWalkDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();
            return walk;
        }
        public async Task<List<Walk>> GetAllAsync()
        {
            return await dbContext.walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk> GetAsync(Guid Id)
        {
            return await dbContext.walks.FirstOrDefaultAsync(x => x.Id == Id);
        }
        public async Task<Walk> UpdateAsync(Guid Id, Walk walk)
        {
            var DomainWalkModel = await dbContext.walks.FirstOrDefaultAsync(x => x.Id == Id);
            if (DomainWalkModel == null)
            {
                return null;
            }
            DomainWalkModel.Name = walk.Name;
            DomainWalkModel.Description = walk.Description;
            DomainWalkModel.LengthInKm = walk.LengthInKm;
            DomainWalkModel.WalkImageUrl = walk.WalkImageUrl;
            DomainWalkModel.DifficultyId = walk.DifficultyId;
            DomainWalkModel.RegionId = walk.RegionId;
            //dbContext.walks.Update(walk);
            await dbContext.SaveChangesAsync();
            return DomainWalkModel;
        }
        public async Task<Walk> DeleteAsync(Guid Id)
        {
            var DomainModelWalk = await dbContext.walks.FirstOrDefaultAsync(x => x.Id == Id);
            if (DomainModelWalk == null)
            {
                return null;
            }

            dbContext.walks.Remove(DomainModelWalk);
            await dbContext.SaveChangesAsync();
            return DomainModelWalk;
        }

        public async Task<List<Walk>> FilterAsync(string? filterOn = null, string? searchText = null)
        {
            var walks = dbContext.walks.Include("Difficulty").Include("Region").AsQueryable();

            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(searchText))
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(searchText));
                }
                else if (filterOn.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.LengthInKm.Contains(searchText));
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(searchText));
                }
            }

            return await walks.ToListAsync();
        }

        public Task<List<Walk>> SortAsync(string? sortBy = null, bool? isAssending = true)
        {
            var walks = dbContext.walks.Include("Difficulty").Include("Region").AsQueryable();

            if(!string.IsNullOrWhiteSpace(sortBy))
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (bool)isAssending ? walks.OrderBy(x=>x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if(sortBy.Equals("LengthInKm", StringComparison.OrdinalIgnoreCase))
                {
                    walks = (bool)isAssending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            return walks.ToListAsync();
        }

        public async Task<List<Walk>> PaginationAsync(int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dbContext.walks.Include("Difficulty").Include("Region").AsQueryable();

            var skipResult = (pageNumber -1 ) * pageSize;

            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
        }

        public Task<List<Walk>> FiSoPageAsync()
        {
            throw new NotImplementedException();
        }
    }
}
