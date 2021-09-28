using System.ComponentModel.DataAnnotations;

namespace phonebook_server.Dtos
{
    public class AddPhonebookDto
    {
        [Required]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Name: Max lenght is 20 chars")]
        public string Name { get; set; }

        [Required]
        [MaxLength(20, ErrorMessage = "Surname: Max lenght is 20 chars")]
        public string Surname { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Phones must contains only digits")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Home Phone Number must consist of 10 digits")]
        public string HomePhoneNumber { get; set; }

        [Required]
        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Phones must contains only digits")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Cell Phone Number must consist of 10 digits")]
        public string CellPhoneNumber { get; set; }

        [Required(ErrorMessage = "District is Mandatory")]
        public int DistrictId { get; set; }
    }
}