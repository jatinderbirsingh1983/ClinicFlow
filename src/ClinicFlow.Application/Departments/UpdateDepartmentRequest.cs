using System.ComponentModel.DataAnnotations;

namespace ClinicFlow.Application.Departments;

public class UpdateDepartmentRequest
{
    [Required]
    [StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public bool IsActive { get; set; }
}
