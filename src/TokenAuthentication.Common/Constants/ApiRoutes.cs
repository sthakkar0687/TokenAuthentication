using System;
using System.Collections.Generic;
using System.Text;

namespace TokenAuthentication.Common.Constants
{
    public static class ApiRoutes
    {
        const string BASEURL = "http://localhost:54347/api";
        public static class Employees
        {
            public static string GetAll = $"{BASEURL}/Employees";
        }
        public static class Authentication
        {
            public static string Login = $"{BASEURL}/Auth/LoginAsync";
        }
        public static class User
        {
            public static string Create = $"{BASEURL}/User";
            public static string GetUserIdByEmailAsync = $"{BASEURL}/User/GetUserIdByEmailAsync/";
        }
        public static class Role
        {
            public static string Create = $"{BASEURL}/Role";
            public static string GetRoleIdByName = $"{BASEURL}/Role/GetRoleIdByNameAsync/";
        }
        public static class UserRole
        {
            public static string Create = $"{BASEURL}/UserRole";
            
        }
    }
}
