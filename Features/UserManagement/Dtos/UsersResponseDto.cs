using System;
using System.Collections.Generic;

namespace LMS.Features.UserManagement.Dtos
{
    public class UsersResponseDto
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public string FirstName { get; set; }
        public string? LastName { get; set; }

        public List<string> Roles { get; set; } = new(); // Handling multiple roles
        public string Type { get; set; }

        public string? Designation { get; set; }
        public string? EmploymentType { get; set; }
        public string? EmploymentStatus { get; set; }
        public string? Qualification { get; set; }

        public List<string> Departments { get; set; } = new(); // Handling multiple departments

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
