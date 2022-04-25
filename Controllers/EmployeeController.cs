using EmployeeAPI.Data;
using EmployeeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly UserDbContext _context;

        public EmployeeController(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }

        [HttpPost("add_employee")]
        public IActionResult AddEmployee([FromBody] EmployeeModel employeeObj)
        {
            if (employeeObj == null)
            {
                return BadRequest();
            }
            else
            {
                _context.employeeModels.Add(employeeObj);
                _context.SaveChanges();
                return Ok(new 
                {
                    StatusCode = 200,
                    Message = "Employee Added Successfully"
                });            
            }
        }
        [HttpPut("update_employee")]
        public IActionResult UpdateEmployee([FromBody] EmployeeModel employeeObj)
        {
            if(employeeObj == null)
            { 
                return BadRequest();
            }
            var user = _context.employeeModels.AsNoTracking().FirstOrDefault(x => x.Id == employeeObj.Id);

            if(user == null)
            {
                return NotFound(new
                {
                    StatusCode = 204,
                    Message = "User Not Found"
                });
            }
            else
            {
                _context.Entry(employeeObj).State = EntityState.Modified;
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCode = 200,
                    Message = "Employee Updated Successfully!"       
                });
            }
        }

        [HttpDelete("delete_employee/{id}")]
        public IActionResult DeleteEmployee(int id)
        {
            var user = _context.employeeModels.Find(id);
            if(user == null)
            { 
                return NotFound(new
                {
                    Status = 404,
                    Message = "User Not Found!"
                });
            }
            else
            {
                _context.Remove(user);
                _context.SaveChanges();
                return Ok(new
                {
                    Statuscode = 200,
                    Message = "Employee deleted!"
                });
            }
        }

        [HttpGet("get_all_employees")]
        public IActionResult GetAllEmployee()
        {
            var employee = _context.employeeModels.AsQueryable();
            return Ok(new
            {
                StatusCode = 200,
                EmployeeDetails = employee
            });
        }


        [HttpGet("get_employees/id")]
        public IActionResult GetEmployee(int id)
        {
            var employee = _context.employeeModels.Find(id);

            if(employee == null)
            {
                return BadRequest(new
                {
                    StatusCode = 404,
                    Message = "User Not found!"
                });
            }
            else
            {
                return Ok(new 
                {
                    StatusCode = 200,
                    EmployeeDetails = employee
                });
            } 
        }
            
        [HttpGet("get_employees/Name")]
        public IActionResult GetEmployeeByName(string FirstName)
        {
            var employee = _context.employeeModels.FirstOrDefault(x => x.FirstName == FirstName);

            if (employee == null)
            {
                return BadRequest(new
                {
                    StatusCode = 404,
                    Message = "User Not found!"
                });
            }
            else
            {
                return Ok(new
                {
                    StatusCode = 200,
                    EmployeeDetails = employee
                });
            }
        }
    }
}

       
           
