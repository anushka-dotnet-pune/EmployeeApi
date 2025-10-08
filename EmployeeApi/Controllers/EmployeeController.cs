using EmployeeApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private static List<Employee> Employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Anushka", Department = "IT", MobileNo = "9876543210", Email = "anushka@example.com" },
            new Employee { Id = 2, Name = "Shreya", Department = "HR", MobileNo = "9999999999", Email = "shreya@example.com" }
        };

        
        [HttpGet]
        public ActionResult<List<Employee>> GetAllEmployees() => Employees;

        
        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById([FromRoute] int id)
        {
            var emp = Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();
            return emp;
        }

        
        [HttpGet("department")]
        public ActionResult<List<Employee>> GetEmployeesByDept([FromQuery] string dept)
        {
            var emps = Employees.Where(e => e.Department.Equals(dept, StringComparison.OrdinalIgnoreCase)).ToList();
            return emps;
        }

        
        [HttpPost]
        public ActionResult AddEmployee([FromBody] Employee employee)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (Employees.Any(e => e.Id == employee.Id)) return Conflict("Employee with same ID exists.");

            Employees.Add(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
        }

        
        [HttpPut("{id}")]
        public ActionResult UpdateEmployee([FromRoute] int id, [FromBody] Employee employee)
        {
            var emp = Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();

            emp.Name = employee.Name;
            emp.Department = employee.Department;
            emp.MobileNo = employee.MobileNo;
            emp.Email = employee.Email;

            return NoContent();
        }

        
        [HttpPatch("email/{id}")]
        public ActionResult UpdateEmployeeEmail([FromRoute] int id, [FromQuery] string email)
        {
            var emp = Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();

            emp.Email = email;
            return NoContent();
        }

        
        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee([FromRoute] int id)
        {
            var emp = Employees.FirstOrDefault(e => e.Id == id);
            if (emp == null) return NotFound();

            Employees.Remove(emp);
            return NoContent();
        }

        
        [HttpHead]
        public ActionResult HeadRequest() => Ok();

        
        [HttpOptions]
        public ActionResult OptionsRequest() => Ok(new string[] { "GET", "POST", "PUT", "PATCH", "DELETE", "HEAD", "OPTIONS" });
    }
}
