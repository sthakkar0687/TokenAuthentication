using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Dtos.Authentication
{
    public class RegistrationResponseDto
    {
        public Guid UserId { get; set; }
        public string Email { get; set; }
    }
}
