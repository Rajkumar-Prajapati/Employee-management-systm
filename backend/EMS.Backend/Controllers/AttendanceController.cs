using EMS.Backend.Data;
using EMS.Backend.DTOs;
using EMS.Backend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EMS.Backend.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll([FromQuery] DateTime? date, [FromQuery] int? employeeId)
        {
            var query = _context.Attendances.Include(a => a.Employee).AsQueryable();

            if (date.HasValue)
                query = query.Where(a => a.Date.Date == date.Value.Date);

            if (employeeId.HasValue)
                query = query.Where(a => a.EmployeeId == employeeId.Value);

            var records = await query
                .OrderByDescending(a => a.Date)
                .Select(a => new AttendanceResponseDto
                {
                    Id = a.Id,
                    EmployeeId = a.EmployeeId,
                    EmployeeName = a.Employee != null ? a.Employee.FirstName + " " + a.Employee.LastName : "-",
                    Date = a.Date,
                    CheckIn = a.CheckIn,
                    CheckOut = a.CheckOut,
                    Status = a.Status
                })
                .ToListAsync();

            return Ok(records);
        }

        [HttpPost("mark")]
        public async Task<ActionResult> Mark(AttendanceMarkDto dto)
        {
            var existing = await _context.Attendances.FirstOrDefaultAsync(
                a => a.EmployeeId == dto.EmployeeId && a.Date.Date == dto.Date.Date);

            if (existing != null)
            {
                existing.CheckIn = dto.CheckIn;
                existing.CheckOut = dto.CheckOut;
                existing.Status = dto.Status;
            }
            else
            {
                _context.Attendances.Add(new Attendance
                {
                    EmployeeId = dto.EmployeeId,
                    Date = dto.Date,
                    CheckIn = dto.CheckIn,
                    CheckOut = dto.CheckOut,
                    Status = dto.Status
                });
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Attendance saved" });
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var record = await _context.Attendances.FindAsync(id);
            if (record == null) return NotFound(new { message = "Record not found" });

            _context.Attendances.Remove(record);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
