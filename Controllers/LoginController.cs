using EmployeeAPI.Data;
using EmployeeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserDbContext _context;
        public LoginController(UserDbContext userDbContext)
        {
            _context = userDbContext;
        }


        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var userdetails = _context.userModels.AsQueryable();
            return Ok(userdetails);
        }

        [HttpPost("signup")]
        public IActionResult SignUp([FromBody] UserModel userObj)
        {
            if(userObj == null)
            {
                return BadRequest();
            }
            else
            {
                _context.userModels.Add(userObj);
                _context.SaveChanges();
                return Ok(new
                {
                    StatusCodes = 200,
                    Message = "User Add Successfully!"
                });
            }
        }

        [HttpPost("login")]
        public IActionResult Login ([FromBody] UserModel userObj)
        {
            if (userObj == null)
            {
                return BadRequest();
            }
            else
            {
                var User = _context.userModels.Where(a => a.UserName == userObj.UserName && a.Password == userObj.Password).FirstOrDefault();
                if (User != null)
                {
                    return Ok(new
                    {
                        StatusCode = 200,
                        Message = "LoginIn Successfully!",
                       
                    });
                }
                else
                {
                    return NotFound(new
                    {
                        StatusCode = 404,
                        Message = "User Not found"
                    });  
                }
            }
        }
    }
}
