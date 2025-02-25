using Dynamic_RBAMS.Features.CampusManagement;
using Dynamic_RBAMS.Features.UserManagement.Models;

namespace Dynamic_RBAMS.Features.Common.Models
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
