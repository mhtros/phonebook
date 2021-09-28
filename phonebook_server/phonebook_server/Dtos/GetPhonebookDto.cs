using phonebook_server.Models;

namespace phonebook_server.Dtos
{
    public class GetPhonebookDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string HomePhoneNumber { get; set; }
        public string CellPhoneNumber { get; set; }
        public District District { get; set; }
    }
}