using LMS.Features.DepartmentManagement;
using System.ComponentModel.DataAnnotations;
using LMS.Features.BatchManagement;

namespace LMS.Features.ProgramManagement;

public class Programs
{
    [Key]
    public int ProgramId { get; set; }
    [Required]
    public string ProgramName { get; set; }

    public string Description { get; set; }
    [Required]
    public string Code { get; set; }

    public string Duration { get; set; }

    [Required]
    public string DegreeType { get; set; }
    [Required]
    public int CreditRequired { get; set; }
    public bool IsDeleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime UpdatedAt { get; set; }

    // Foreign keys and Navigation properties are defined here

    public Department Department { get; set; }
    [Required]
    public int DepartmentId { get; set; }

    public ICollection<ProgramBatch> ProgramBatches { get; set; } = new List<ProgramBatch>();

}
