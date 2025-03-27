using LMS.Features.CampusManagement;
using LMS.Features.UserManagement.Models;

namespace LMS.Features.Common.Models
{
    // Junction Table (Many-to-Many)
    public class FacultyCampus
    { 
        public int Id { get; set; }
        public string FacultyId { get; set; }
        public Faculty Faculty { get; set; }

        public int CampusId { get; set; }
        public Campus Campus { get; set; }
    }
}
