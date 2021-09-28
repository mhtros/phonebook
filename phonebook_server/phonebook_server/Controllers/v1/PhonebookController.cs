using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using phonebook_server.Classes;
using phonebook_server.Classes.Pagination;
using phonebook_server.Data;
using phonebook_server.Dtos;
using phonebook_server.Extensions;
using phonebook_server.Models;

namespace phonebook_server.Controllers.v1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PhonebookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public PhonebookController(IConfiguration configuration, DataContext context, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id:int}")]
        public async Task<OkObjectResult> Get(int id)
        {
            var phonebook = await _context.Phonebooks.Include(p => p.District).FirstOrDefaultAsync(p => p.Id == id);
            var dto = _mapper.Map<GetPhonebookDto>(phonebook);
            return Ok(dto);
        }

        [HttpGet]
        public async Task<OkObjectResult> Search([FromQuery] PaginationParameters pagination,
            [FromQuery] SearchFilter filter)
        {
            var query = _context.Phonebooks.Include(p => p.District)
                .ProjectTo<GetPhonebookDto>(_mapper.ConfigurationProvider);

            query = PhonebookAddFilters(filter, query);
            query = PhonebookAddOrderBy(filter, query);

            var pb =
                await PagedList<GetPhonebookDto>.CreateAsync(query, pagination.PageNumber, pagination.PageSize);

            Response.AddPaginationHeader(pb.CurrentPage, pb.PageSize, pb.TotalCount,
                pb.TotalPages);

            return Ok(pb);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddPhonebookDto dto)
        {
            var newPhonebook = _mapper.Map<Phonebook>(dto);
            await _context.Phonebooks.AddAsync(newPhonebook);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("edit")]
        public async Task<IActionResult> Edit(EditPhoneBookDto dto)
        {
            var newPhonebook = _mapper.Map<Phonebook>(dto);
            var savedPhonebook = await _context.Phonebooks.FindAsync(dto.Id);

            var isDistrictInvalid = await CheckDistrictValidity(dto.DistrictId);
            if (isDistrictInvalid) return NotFound("The given district is not in the system");

            savedPhonebook.Name = newPhonebook.Name;
            savedPhonebook.Surname = newPhonebook.Surname;
            savedPhonebook.Email = newPhonebook.Email;
            savedPhonebook.HomePhoneNumber = newPhonebook.HomePhoneNumber;
            savedPhonebook.CellPhoneNumber = newPhonebook.CellPhoneNumber;
            savedPhonebook.DistrictId = newPhonebook.DistrictId;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var phonebook = await _context.Phonebooks.FindAsync(id);
            _context.Phonebooks.Remove(phonebook);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task<bool> CheckDistrictValidity(int districtId)
        {
            var district = await _context.Districts.FindAsync(districtId);
            return district == null;
        }

        private static IQueryable<GetPhonebookDto> PhonebookAddOrderBy(SearchFilter filter,
            IQueryable<GetPhonebookDto> query)
        {
            if (string.IsNullOrWhiteSpace(filter.OrderBy)) return query;

            var districtFilter = filter.OrderBy.Split('.');

            if (districtFilter.Length > 1)
            {
                if (typeof(Phonebook).HasProperty(districtFilter.First()))
                    query = query.PhonebookOrderBy(filter.OrderBy, filter.OrderByDirection);
            }
            else if (typeof(Phonebook).HasProperty(filter.OrderBy))
            {
                query = query.PhonebookOrderBy(filter.OrderBy, filter.OrderByDirection);
            }

            return query;
        }

        private IQueryable<GetPhonebookDto> PhonebookAddFilters(SearchFilter filter, IQueryable<GetPhonebookDto> query)
        {
            if (filter.FilterProperties == null) return query;

            // Take(mfn) to ensure that nobody will spam unnecessary filters
            var mfn = _configuration.GetSection("MaxNumberOfFilters").Get<int>();

            List<string> nameFil = null,
                surnameFil = null,
                emailFil = null,
                cellPhoneFil = null,
                homePhoneFil = null,
                disNameFil = null,
                disPostFil = null;

            var dictionary = new Dictionary<string, Action<string>>
            {
                { "name", value => Extensions.Extensions.AddToList(ref nameFil, value) },
                { "surname", value => Extensions.Extensions.AddToList(ref surnameFil, value) },
                { "email", value => Extensions.Extensions.AddToList(ref emailFil, value) },
                { "cellphonenumber", value => Extensions.Extensions.AddToList(ref cellPhoneFil, value) },
                { "homephonenumber", value => Extensions.Extensions.AddToList(ref homePhoneFil, value) },
                { "district.name", value => Extensions.Extensions.AddToList(ref disNameFil, value) },
                { "district.postcode", value => Extensions.Extensions.AddToList(ref disPostFil, value) }
            };

            foreach (var filterProperty in filter.FilterProperties.Take(mfn))
            {
                var districtProperty = filterProperty.PropertyName.Split('.');

                var regularValidation = !string.IsNullOrWhiteSpace(filterProperty.PropertyValue) &&
                                        typeof(Phonebook).HasProperty(filterProperty.PropertyName);

                var districtValidation = !string.IsNullOrWhiteSpace(filterProperty.PropertyValue) &&
                                         typeof(Phonebook).HasProperty(districtProperty.First());

                var isValid = districtProperty.Length > 1 ? districtValidation : regularValidation;

                if (!isValid) continue;
                if (!dictionary.ContainsKey(filterProperty.PropertyName)) continue;

                dictionary[filterProperty.PropertyName](filterProperty.PropertyValue.ToLower());
            }

            if (nameFil != null) query = query.ApplyPhoneBookFilter("name", nameFil.ToArray());
            if (surnameFil != null) query = query.ApplyPhoneBookFilter("surname", surnameFil.ToArray());
            if (emailFil != null) query = query.ApplyPhoneBookFilter("email", emailFil.ToArray());
            if (cellPhoneFil != null) query = query.ApplyPhoneBookFilter("cellphonenumber", cellPhoneFil.ToArray());
            if (homePhoneFil != null) query = query.ApplyPhoneBookFilter("homephonenumber", homePhoneFil.ToArray());
            if (disNameFil != null) query = query.ApplyPhoneBookFilter("district.name", disNameFil.ToArray());
            if (disPostFil != null) query = query.ApplyPhoneBookFilter("district.postcode", disPostFil.ToArray());

            return query;
        }
    }
}