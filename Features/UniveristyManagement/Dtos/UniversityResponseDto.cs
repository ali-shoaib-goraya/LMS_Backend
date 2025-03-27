namespace LMS.Features.UniveristyManagement.Dtos
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

