using phonebook_server.Interfaces;

namespace phonebook_server.Models
{
    public class District : IDistinctable
    {
        public string PostCode { get; set; }
        public string Name { get; set; }
    }
}