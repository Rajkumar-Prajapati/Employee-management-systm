using System.ComponentModel.DataAnnotations.Schema;

namespace EMS.Backend.Models
{
    public class Attendance
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public Employee? Employee { get; set; }

        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }

        // Present / Absent / Leave / HalfDay
        public string Status { get; set; } = "Present";
    }
}
