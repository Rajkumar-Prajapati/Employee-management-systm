using EMS.Backend.Data;
using EMS.Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IReportService _reportService;

        public ReportsController(ApplicationDbContext context, IReportService reportService)
        {
            _context = context;
            _reportService = reportService;
        }

        [HttpGet("employees/pdf")]
        public async Task<IActionResult> EmployeePdf()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();
            var bytes = _reportService.GenerateEmployeePdf(employees);
            return File(bytes, "application/pdf", "employee-directory.pdf");
        }

        [HttpGet("employees/excel")]
        public async Task<IActionResult> EmployeeExcel()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();
            var bytes = _reportService.GenerateEmployeeExcel(employees);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "employee-directory.xlsx");
        }

        [HttpGet("salary/pdf")]
        public async Task<IActionResult> SalaryPdf()
        {
            var employees = await _context.Employees.Include(e => e.Department).ToListAsync();
            var bytes = _reportService.GenerateSalaryPdf(employees);
            return File(bytes, "application/pdf", "salary-report.pdf");
        }

        [HttpGet("attendance/excel")]
        public async Task<IActionResult> AttendanceExcel([FromQuery] DateTime? from, [FromQuery] DateTime? to)
        {
            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();

            if (from.HasValue) query = query.Where(a => a.Date >= from.Value.Date);
            if (to.HasValue) query = query.Where(a => a.Date <= to.Value.Date);

            var records = await query.OrderBy(a => a.Date).ToListAsync();
            var bytes = _reportService.GenerateAttendanceExcel(records);
            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "attendance-report.xlsx");
        }
    }
}
