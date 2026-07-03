using EMS.Backend.Models;

namespace EMS.Backend.Services
{
    public interface IReportService
    {
        byte[] GenerateEmployeePdf(List<Employee> employees);
        byte[] GenerateEmployeeExcel(List<Employee> employees);
        byte[] GenerateAttendanceExcel(List<Attendance> records);
        byte[] GenerateSalaryPdf(List<Employee> employees);
    }
}
