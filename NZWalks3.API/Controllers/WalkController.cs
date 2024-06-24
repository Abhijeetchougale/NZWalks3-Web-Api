using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks3.API.CustomeValidation;
using NZWalks3.API.DTOs;
using NZWalks3.API.Model.Domain;
using NZWalks3.API.Repository;

namespace NZWalks3.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalkController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalkController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        //POST METHOD
        //POST::/api/post

        [HttpPost]
        [ValidateModel]

        public async Task<IActionResult> CreateWalk (AddWalkDto addWalkDto)
        {
            //Map AddWalkDto TO Walk Model 
            var DomainWalkModel = mapper.Map<Walk>(addWalkDto);
            //Pass DomainWalkModel to Repository
            DomainWalkModel = await walkRepository.CreateAsync(DomainWalkModel);
            //Map DomainWalkModel TO WalkDto Model
            return Ok(mapper.Map<WalkDto>(DomainWalkModel));

        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var DomainWalkModels = await walkRepository.GetAllAsync();
            //Map DomainWalkModels TO WalkDto Model
            return Ok(mapper.Map<IEnumerable<WalkDto>>(DomainWalkModels));
        }

        [HttpGet ("{Id:Guid}")]

        public async Task<IActionResult>GetById(Guid Id)
        {
            var DomainWalkModel = await walkRepository.GetAsync(Id);
            //Map DomainWalkModels TO WalkDto Model
            return Ok (mapper.Map<WalkDto>(DomainWalkModel));  
        }

        [HttpPut ("{updatedId:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> UpdateWalk(Guid Id, UpdateWalkDto updateWalkDto)
        {
            var DomainModelWalk = mapper.Map<Walk>(updateWalkDto);

            if (DomainModelWalk == null)
            {
                return NotFound();
            }
            DomainModelWalk = await walkRepository.UpdateAsync(Id,DomainModelWalk);

            return Ok(mapper.Map<WalkDto>(DomainModelWalk));
        }

        [HttpDelete("{Id:Guid}")]
        public async Task<IActionResult> Delete(Guid Id)
        {
            var DomainModelWalk = await walkRepository.DeleteAsync(Id);
            if (DomainModelWalk == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto> (DomainModelWalk));
        }

        [HttpGet("FilterByNameLengthDescription")]
        public async Task<IActionResult> GetAllFilter([FromQuery] string? filterOn, [FromQuery] string? searchText)
        {
            var DomainModel = await walkRepository.FilterAsync(filterOn, searchText);
            return Ok(mapper.Map<List<WalkDto>>(DomainModel));
        }

        [HttpGet ("SortByNameAndLength")]
        public async Task<IActionResult> GetAllSortByName([FromQuery]string? sortOn ,[FromQuery] bool? isAssending )
        {
            var DomainModelWalk = await walkRepository.SortAsync(sortOn, isAssending ??  true);

            return Ok(mapper.Map<List<WalkDto>>(DomainModelWalk));
        }

        [HttpGet ("Pagination")]
        public async Task<IActionResult> pagination([FromQuery] int pageNumber, [FromQuery] int pageSize)
        {
            var DomainModelWalk = await walkRepository.PaginationAsync(pageNumber, pageSize);   

            return Ok(mapper.Map<List<WalkDto>>(DomainModelWalk));
        }
    }
}
