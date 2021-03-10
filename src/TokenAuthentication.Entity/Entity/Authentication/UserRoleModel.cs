using System;

namespace TokenAuthentication.Entity.Authentication
{
    public class UserRoleModel
    {
        public Guid UserId { get; set; }
        public Guid RoleId { get; set; }
    }
}
