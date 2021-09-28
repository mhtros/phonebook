using AutoMapper;
using phonebook_server.Dtos;
using phonebook_server.Models;

namespace phonebook_server.Classes
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            CreateMap<AddPhonebookDto, Phonebook>();
            CreateMap<EditPhoneBookDto, Phonebook>();
            CreateMap<Phonebook, GetPhonebookDto>();
            CreateMap<District, GetDistrictDto>();
        }
    }
}