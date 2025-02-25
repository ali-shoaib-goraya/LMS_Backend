using System.ComponentModel.DataAnnotations;
using Dynamic_RBAMS.Features.DepartmentManagement;
using Dynamic_RBAMS.Features.UserManagement.Models;

namespace Dynamic_RBAMS.Features.Common.Models
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
