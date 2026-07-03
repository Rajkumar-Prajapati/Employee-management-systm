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
    public class DepartmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var departments = await _context.Departments
                .Select(d => new DepartmentResponseDto
                {
                    Id = d.Id,
                    Name = d.Name,
                    Description = d.Description,
                    EmployeeCount = d.Employees.Count
                })
                .ToListAsync();

            return Ok(departments);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = "Department not found" });
            return Ok(department);
        }

        [HttpPost]
        public async Task<ActionResult> Create(DepartmentDto dto)
        {
            var department = new Department { Name = dto.Name, Description = dto.Description };
            _context.Departments.Add(department);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = department.Id }, department);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, DepartmentDto dto)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = "Department not found" });

            department.Name = dto.Name;
            department.Description = dto.Description;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            if (department == null) return NotFound(new { message = "Department not found" });

            var hasEmployees = await _context.Employees.AnyAsync(e => e.DepartmentId == id);
            if (hasEmployees)
                return BadRequest(new { message = "Cannot delete department with assigned employees" });

            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
