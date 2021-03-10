using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace TokenAuthentication.Entity.Entity
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; }

        [Column(TypeName = "date")]
        public DateTime DOB { get; set; }
    }
}
