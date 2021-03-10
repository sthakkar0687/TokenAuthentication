using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Dtos.Authentication
{
    public class RoleResponseDto
    {
        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
