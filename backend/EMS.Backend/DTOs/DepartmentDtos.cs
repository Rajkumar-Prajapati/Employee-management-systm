using System.ComponentModel.DataAnnotations;

namespace EMS.Backend.DTOs
{
    public class DepartmentDto
    {
        [Required] public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class DepartmentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int EmployeeCount { get; set; }
    }
}
