using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentAPIController(IStudentService studentService)
        {
            this._studentService = studentService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            QueryResponse<Student> response = await _studentService.GetAll();
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Student student)
        {
            SingleResponse<Student> response = await _studentService.GetStudentByRegister(student.Register, student.Passcode);

            if (!response.Success)
            {
                return NotFound();
            }

            string token = TokenService.GenerateToken(response.Data.Register, "Student");
            User user = new User()
            {
                ID = response.Data.ID,
                Name = response.Data.StudentName,
                Role = "Student",
                Token = token,
                UserName = response.Data.Register
            };

            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)
        {
            SingleResponse<int> response = await _studentService.Insert(student);
            return Ok(response);
        }

      
    }
}
