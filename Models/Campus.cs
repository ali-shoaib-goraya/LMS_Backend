using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Dynamic_RBAMS.Models
{
    public class Campus  
    {
        public int CampusId { get; set; } 
        public required string CampusName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Notes { get; set; } 
        public string Type { get; set; } // "Main Campus" or "Sub Campus"
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; }

        // Navigation properties defined here
        public int UniversityId { get; set; }
        [JsonIgnore]  // Prevents circular reference
        public University University { get; set; }
        [JsonIgnore]  // Prevents circular reference
        public ICollection<School> Schools { get; set; } = new List<School>();
        [JsonIgnore]  // Prevents circular reference
        public virtual ICollection<FacultyCampus> FacultyCampuses { get; set; }
    }
}
