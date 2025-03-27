﻿using System.Text.Json.Serialization;
using LMS.Features.CampusManagement;
namespace LMS.Features.UniveristyManagement
{
    public class University
    {
        public int UniversityId { get; set; }
        public string UniversityName { get; set; }

        public string Address { get; set; }
        public bool IsDeleted { get; set; } = false; // Soft delete flag

        [JsonIgnore]  // Prevents circular reference
        public ICollection<Campus> Campuses { get; set; } = new List<Campus>();
    }
}
