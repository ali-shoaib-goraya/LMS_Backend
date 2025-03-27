using System;
namespace LMS.Features.CampusManagement.Dtos
{
    public class CampusResponseDto
    {
        public int CampusId { get; set; }
        public string CampusName { get; set; }
        public string ShortName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Notes { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // ✅ Constructor to Convert Campus Entity to DTO
        public CampusResponseDto(Campus campus)
        {
            CampusId = campus.CampusId;
            CampusName = campus.CampusName;
            ShortName = campus.ShortName;
            Address = campus.Address;
            City = campus.City;
            Notes = campus.Notes;
            Type = campus.Type;
            CreatedAt = campus.CreatedAt;
            UpdatedAt = campus.UpdatedAt;
        }

        // ✅ Empty Constructor for Serialization
        public CampusResponseDto() { }
    }
}
