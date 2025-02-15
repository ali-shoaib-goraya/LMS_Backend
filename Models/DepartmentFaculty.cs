
using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Models
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
