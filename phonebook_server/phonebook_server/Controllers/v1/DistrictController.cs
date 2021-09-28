using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using phonebook_server.Data;
using phonebook_server.Dtos;

namespace phonebook_server.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class DistrictController : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public DistrictController(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<OkObjectResult> GetDistricts()
        {
            var districts = await _context.Districts
                .ProjectTo<GetDistrictDto>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return Ok(districts);
        }
    }
}