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
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/employees?search=&departmentId=&status=&page=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult> GetAll(
            [FromQuery] string? search,
            [FromQuery] int? departmentId,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Employees.Include(e => e.Department).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(e =>
                    e.FirstName.ToLower().Contains(search) ||
                    e.LastName.ToLower().Contains(search) ||
                    e.Email.ToLower().Contains(search) ||
                    e.Designation.ToLower().Contains(search));
            }

            if (departmentId.HasValue)
                query = query.Where(e => e.DepartmentId == departmentId.Value);

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(e => e.Status == status);

            var total = await query.CountAsync();

            var employees = await query
                .OrderByDescending(e => e.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => ToDto(e))
                .ToListAsync();

            return Ok(new { total, page, pageSize, data = employees });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeResponseDto>> GetById(int id)
        {
            var employee = await _context.Employees.Include(e => e.Department)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null) return NotFound(new { message = "Employee not found" });

            return Ok(ToDto(employee));
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeResponseDto>> Create(EmployeeCreateDto dto)
        {
            if (await _context.Employees.AnyAsync(e => e.Email == dto.Email))
                return BadRequest(new { message = "Email already exists" });

            var employee = new Employee
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone,
                Gender = dto.Gender,
                Address = dto.Address,
                Designation = dto.Designation,
                DepartmentId = dto.DepartmentId,
                Salary = dto.Salary,
                DateOfJoining = dto.DateOfJoining ?? DateTime.UtcNow,
                Status = dto.Status,
                ProfileImageUrl = dto.ProfileImageUrl
            };

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();
            await _context.Entry(employee).Reference(e => e.Department).LoadAsync();

            return CreatedAtAction(nameof(GetById), new { id = employee.Id }, ToDto(employee));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, EmployeeUpdateDto dto)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound(new { message = "Employee not found" });

            if (await _context.Employees.AnyAsync(e => e.Email == dto.Email && e.Id != id))
                return BadRequest(new { message = "Email already in use by another employee" });

            employee.FirstName = dto.FirstName;
            employee.LastName = dto.LastName;
            employee.Email = dto.Email;
            employee.Phone = dto.Phone;
            employee.Gender = dto.Gender;
            employee.Address = dto.Address;
            employee.Designation = dto.Designation;
            employee.DepartmentId = dto.DepartmentId;
            employee.Salary = dto.Salary;
            if (dto.DateOfJoining.HasValue) employee.DateOfJoining = dto.DateOfJoining.Value;
            employee.Status = dto.Status;
            employee.ProfileImageUrl = dto.ProfileImageUrl;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null) return NotFound(new { message = "Employee not found" });

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("bulk-delete")]
        public async Task<ActionResult> BulkDelete([FromBody] List<int> ids)
        {
            var employees = await _context.Employees.Where(e => ids.Contains(e.Id)).ToListAsync();
            _context.Employees.RemoveRange(employees);
            await _context.SaveChangesAsync();
            return Ok(new { deleted = employees.Count });
        }

        [HttpGet("stats")]
        public async Task<ActionResult> GetStats()
        {
            var total = await _context.Employees.CountAsync();
            var active = await _context.Employees.CountAsync(e => e.Status == "Active");
            var inactive = total - active;
            var departments = await _context.Departments.CountAsync();
            var avgSalary = total > 0 ? await _context.Employees.AverageAsync(e => e.Salary) : 0;

            return Ok(new { total, active, inactive, departments, avgSalary });
        }

        private static EmployeeResponseDto ToDto(Employee e) => new()
        {
            Id = e.Id,
            FirstName = e.FirstName,
            LastName = e.LastName,
            Email = e.Email,
            Phone = e.Phone,
            Gender = e.Gender,
            Address = e.Address,
            Designation = e.Designation,
            DepartmentId = e.DepartmentId,
            DepartmentName = e.Department?.Name,
            Salary = e.Salary,
            DateOfJoining = e.DateOfJoining,
            Status = e.Status,
            ProfileImageUrl = e.ProfileImageUrl
        };
    }
}
