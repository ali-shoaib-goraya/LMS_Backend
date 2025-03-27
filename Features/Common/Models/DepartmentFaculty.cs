using System.ComponentModel.DataAnnotations;
using LMS.Features.DepartmentManagement;
using LMS.Features.UserManagement.Models;

namespace LMS.Features.Common.Models
{
    public class DepartmentFaculty
    {
        public int Id { get; set; }
        public string FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int DepartmentId { get; set; } 
        public Department Department { get; set; }
    }
}
