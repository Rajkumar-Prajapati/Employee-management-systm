using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Backend.Models
{
    public class Employee
    {
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? Phone { get; set; }

        [MaxLength(20)]
        public string? Gender { get; set; }

        [MaxLength(150)]
        public string? Address { get; set; }

        [Required, MaxLength(80)]
        public string Designation { get; set; } = string.Empty;

        public int DepartmentId { get; set; }

        [ForeignKey(nameof(DepartmentId))]
        public Department? Department { get; set; }

        [Column(TypeName = "decimal(12,2)")]
        public decimal Salary { get; set; }

        public DateTime DateOfJoining { get; set; } = DateTime.UtcNow;

        [MaxLength(20)]
        public string Status { get; set; } = "Active"; // Active / Inactive

        public string? ProfileImageUrl { get; set; }

        public ICollection<Attendance> AttendanceRecords { get; set; } = new List<Attendance>();
    }
}
