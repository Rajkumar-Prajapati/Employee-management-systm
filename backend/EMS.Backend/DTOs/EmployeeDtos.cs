using System.ComponentModel.DataAnnotations;

namespace EMS.Backend.DTOs
{
    public class EmployeeCreateDto
    {
        [Required] public string FirstName { get; set; } = string.Empty;
        [Required] public string LastName { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        [Required] public string Designation { get; set; } = string.Empty;
        [Required] public int DepartmentId { get; set; }
        [Required] public decimal Salary { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Status { get; set; } = "Active";
        public string? ProfileImageUrl { get; set; }
    }

    public class EmployeeUpdateDto : EmployeeCreateDto
    {
    }

    public class EmployeeResponseDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string Designation { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
        public decimal Salary { get; set; }
        public DateTime DateOfJoining { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
    }
}
