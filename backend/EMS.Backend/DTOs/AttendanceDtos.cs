namespace EMS.Backend.DTOs
{
    public class AttendanceMarkDto
    {
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow.Date;
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string Status { get; set; } = "Present";
    }

    public class AttendanceResponseDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan? CheckIn { get; set; }
        public TimeSpan? CheckOut { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
