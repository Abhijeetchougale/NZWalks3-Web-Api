using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks3.API.CustomeValidation;
using NZWalks3.API.DatabaseConnection;
using NZWalks3.API.DTOs;
using NZWalks3.API.Model.Domain;
using NZWalks3.API.Repository;

namespace NZWalks3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionController(IRegionRepository regionRepository, IMapper mapper )
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult>Get()
        {
            var DomainRegionModels = await regionRepository.GetAllAsync();

            //Map DomainModel To Region DTO
            return Ok (mapper.Map<List<RequestRegionDto>>(DomainRegionModels));
        }

        [HttpGet ("{regionId :Guid}")]
        [Authorize(Roles = "Reader")]

        public async Task<IActionResult> GetByRegionId (Guid regionId)
        {
            var DomainRegionModel = await regionRepository.GetByIdAsync(regionId);
            if (DomainRegionModel == null)
            {
                return NotFound ();
            }
            //Map DomainModel To DTO
            return Ok (mapper.Map<RequestRegionDto>(DomainRegionModel));           
        }

        [HttpPost]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> CreateRegion (AddRequestRegionDto addRequestRegionDto)
        {
            //Map addRequestRegionDto to DomainModel
            var DomainRegionModel = mapper.Map<Region>(addRequestRegionDto);

            DomainRegionModel = await regionRepository.CreateAsync(DomainRegionModel);

            return Ok(DomainRegionModel);
        }

        [HttpPut ("{UpdatedId:Guid}")]
        [ValidateModel]
        [Authorize(Roles = "Writer")]

        public async Task<IActionResult> UpdateRegionById (Guid UpdatedId, UpdateRequestRegionDto updateRequestRegionDto)
        {
            //Map updateRequestRegionDto to Region model
            var DomainRegionModel = mapper.Map<Region> (updateRequestRegionDto);    
           
            DomainRegionModel = await regionRepository.UpadateAsync(UpdatedId, DomainRegionModel);

            if (DomainRegionModel == null)
            {
                return NotFound();
            }

            //Map Region Model to Region DTO model
            return Ok(mapper.Map<RequestRegionDto>(DomainRegionModel));

        }

        [HttpDelete ("{DeletedId:Guid}")]
        [Authorize(Roles = "Writer")]


        public async Task<IActionResult> DeleteById(Guid DeletedId)
        {
            var DomainRegionModel = await regionRepository.DeleteAsync(DeletedId);

            if (DomainRegionModel == null)
            {
                return NotFound();
            }
            //Map Region Model to Region DTO model
            return Ok (mapper.Map<RequestRegionDto>(DomainRegionModel));

           
        }
    }
}
