namespace Dynamic_RBAMS.DTOs
{
    namespace Dynamic_RBAMS.DTOs
    {
        public class DepartmentResponseDto
        {
            public int DepartmentId { get; set; }
            public string DepartmentName { get; set; }
            public string? ShortName { get; set; }
            public string? Vision { get; set; }
            public string Type { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }

            // School id
            public int SchoolId { get; set; }
        }
    }

}
