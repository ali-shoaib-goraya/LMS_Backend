
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
{ 
        public class FacultyCampus 
        {
            public string FacultyId { get; set; } 
            public Faculty Faculty { get; set; }
             
            public int CampusId { get; set; } 
            public Campus Campus { get; set; }
        } 
}
