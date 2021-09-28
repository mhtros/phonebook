using System.Collections.Generic;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using phonebook_server.Interfaces;
using phonebook_server.Models;

namespace phonebook_server.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<District>().HasData(GenerateDistrictData());
            modelBuilder.Entity<Phonebook>().HasData(GeneratePhoneBookData());
        }

        private static IEnumerable<District> GenerateDistrictData()
        {
            var districtsJson = File.ReadAllText("districts.json");
            var districts = JsonConvert.DeserializeObject<List<District>>(districtsJson);
            foreach (var district in AssignIds(districts)) yield return (District)district;
        }

        private static IEnumerable<Phonebook> GeneratePhoneBookData()
        {
            var phonebooksJson = File.ReadAllText("phonebooks.json");
            var phonebooks = JsonConvert.DeserializeObject<List<Phonebook>>(phonebooksJson);
            foreach (var phonebook in AssignIds(phonebooks)) yield return (Phonebook)phonebook;
        }

        private static IEnumerable<IDistinctable> AssignIds(IReadOnlyList<IDistinctable> records)
        {
            for (var i = 0; i < records.Count; i++)
            {
                records[i].Id = i + 1;
                yield return records[i];
            }
        }
    }
}