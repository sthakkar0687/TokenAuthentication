using System;
using System.ComponentModel.DataAnnotations;

namespace TokenAuthentication.Entity.Authentication
{
    public class RoleModel
    {
        public Guid RoleId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string RoleName { get; set; }
    }
}
