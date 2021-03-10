using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TokenAuthentication.API.Authorization
{
    public class MinimumAgeRequirement : IAuthorizationRequirement
    {
        public  int MinimumAge;

        public MinimumAgeRequirement(int age)
        {
            MinimumAge = age;
        }
    }
}
