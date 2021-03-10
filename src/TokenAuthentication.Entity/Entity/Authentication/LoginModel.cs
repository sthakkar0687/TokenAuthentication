using System.ComponentModel.DataAnnotations;

namespace TokenAuthentication.Entity.Authentication
{
    public class LoginModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string UserName { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 6)]
        public string Password { get; set; }
    }
}
