using Dynamic_RBAMS.Models;

namespace Dynamic_RBAMS.DTOs
{
    namespace Dynamic_RBAMS.DTOs
    {
        public class UniversityResponseDto
        {
            public int Id { get; set; }
            public string UniversityName { get; set; }
            public string Address { get; set; }

            public UniversityResponseDto(University university)
            {
                Id = university.UniversityId;
                UniversityName = university.UniversityName;
                Address = university.Address;
            }
        }
    }
}

