using System;
using System.Linq;
using NinjaNye.SearchExtensions;
using phonebook_server.Classes;
using phonebook_server.Dtos;

namespace phonebook_server.Extensions
{
    public static class PhonebookExtension
    {
        public static IQueryable<GetPhonebookDto> ApplyPhoneBookFilter(this IQueryable<GetPhonebookDto> query,
            string property, string[] values)
        {
            query = property.ToLower() switch
            {
                "name" => query.Search(p => p.Name).Containing(values),
                "surname" => query.Search(p => p.Surname).Containing(values),
                "email" => query.Search(p => p.Email).Containing(values),
                "cellphonenumber" => query.Search(p => p.CellPhoneNumber).Containing(values),
                "homephonenumber" => query.Search(p => p.HomePhoneNumber).Containing(values),
                "district.name" => query.Search(p => p.District.Name).Containing(values),
                "district.postcode" => query.Search(p => p.District.PostCode).Containing(values),
                _ => query
            };

            return query;
        }

        public static IQueryable<GetPhonebookDto> PhonebookOrderBy(this IQueryable<GetPhonebookDto> query,
            string property, int direction)
        {
            query = property.ToLower() switch
            {
                "name" => query.OrderBy(x => x.Name),
                "surname" => query.OrderBy(x => x.Surname),
                "email" => query.OrderBy(x => x.Email),
                "cellphonenumber" => query.OrderBy(x => x.CellPhoneNumber),
                "homephonenumber" => query.OrderBy(x => x.HomePhoneNumber),
                "district.name" => query.OrderBy(p => p.District.Name),
                "district.postcode" => query.OrderBy(p => p.District.PostCode),
                _ => query
            };

            if (!Enum.IsDefined(typeof(OrderByDirection), direction)) return query;

            return direction switch
            {
                (int)OrderByDirection.Asc => query,
                (int)OrderByDirection.Desc => query.Reverse(),
                _ => query
            };
        }
    }
}