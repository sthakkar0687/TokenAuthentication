using System;
using System.ComponentModel.DataAnnotations;

namespace TokenAuthentication.Dtos
{
    public class EmployeeDto
    {
        public Guid EmployeeId { get; set; }

        [Required]
        public string EmployeeName { get; set; }

        [Required]
        [Range(15,100)]
        public int Age { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string Email { get; set; }
        public Guid DepartmentId { get; set; }
        public string Departmentname { get; set; }
    }
}
