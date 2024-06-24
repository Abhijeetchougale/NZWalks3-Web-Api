using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks3.API.DatabaseConnection;
using NZWalks3.API.Model.Domain;

namespace NZWalks3.API.Repository
{
    public class SQLConnectionRegion : IRegionRepository
    {
        private readonly NZWalkDBContext dbContex;

        public SQLConnectionRegion(NZWalkDBContext DbContex)
        {
            dbContex = DbContex;
        }
        public async Task<List<Region>> GetAllAsync()
        {
            return await dbContex.Regions.ToListAsync();
             
        }
        public async Task<Region> GetByIdAsync(Guid id)
        {
            return await dbContex.Regions.FirstOrDefaultAsync(r => r.Id == id);
        }
        public async Task<Region> CreateAsync(Region region)
        {
            await dbContex.Regions.AddAsync(region);
            await dbContex.SaveChangesAsync();
            return region;   
        }

        public async Task<Region> UpadateAsync(Guid id, Region region)
        {
            var DomainRegionModel = await dbContex.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (DomainRegionModel == null)
            {
                return null;
            }
            DomainRegionModel.Code = region.Code;
            DomainRegionModel.Name = region.Name;
            DomainRegionModel.RegionImageUrl = region.RegionImageUrl;   

            dbContex.SaveChanges();
            return DomainRegionModel;  
        }
        public async Task<Region> DeleteAsync(Guid id)
        {
            var DomainRgionModel =await dbContex.Regions.FirstOrDefaultAsync(x=>x.Id==id);
            if (DomainRgionModel == null)
            {
                return null;
            };

              dbContex.Regions.Remove(DomainRgionModel);
              await dbContex.SaveChangesAsync();

            return DomainRgionModel;

        }

       
    }
}
