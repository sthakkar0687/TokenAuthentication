using System;
using System.ComponentModel.DataAnnotations;

namespace TokenAuthentication.Entity.Authentication
{
    public class RegistrationModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

        [StringLength(50, MinimumLength = 5)]
        [Required]
        public string FirstName { get; set; }

        [StringLength(50, MinimumLength = 4)]
        [Required]
        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [Required]
        public DateTime DOB { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required]
        [StringLength(10, MinimumLength = 10)]
        public string PhoneNumber { get; set; }

        [StringLength(20, MinimumLength = 6)]
        [Required]
        public string Password { get; set; }

        public Guid RoleId { get; set; }
    }
}
