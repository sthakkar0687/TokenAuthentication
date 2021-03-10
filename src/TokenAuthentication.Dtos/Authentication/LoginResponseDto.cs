using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Dtos.Authentication
{
    public class LoginResponseDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiry { get; set; }
    }
}
