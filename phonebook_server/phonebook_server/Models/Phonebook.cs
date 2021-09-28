using phonebook_server.Interfaces;

namespace phonebook_server.Models
{
    public class Phonebook : IDistinctable
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }

        public int DistrictId { get; set; }
        public virtual District District { get; set; }
    }
}