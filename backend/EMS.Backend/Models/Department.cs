using System.ComponentModel.DataAnnotations;

namespace EMS.Backend.Models
{
    public class Department
    {
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(300)]
        public string? Description { get; set; }

        public ICollection<Employee> Employees { get; set; } = new List<Employee>();
    }
}
