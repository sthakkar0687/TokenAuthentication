using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Dtos.Authentication
{
    public class UserRoleResponseDto
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
        public string UserName { get; set; }
        public string RoleName { get; set; }
    }
}
