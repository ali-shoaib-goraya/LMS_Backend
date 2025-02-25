using System.ComponentModel.DataAnnotations;

namespace Dynamic_RBAMS.Features.UniveristyManagement.Dtos
{
    public class CreateUniversityDto
    {
        [Required(ErrorMessage = "University Name is required.")]
        [StringLength(100, ErrorMessage = "University Name can't exceed 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        [StringLength(200, ErrorMessage = "Address can't exceed 200 characters.")]
        public string Address { get; set; }

    }
}
