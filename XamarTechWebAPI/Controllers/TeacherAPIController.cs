using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TeacherAPIController : ControllerBase
    {
        private readonly ITeacherService _teacherService;

        public TeacherAPIController(ITeacherService teacherService)
        {
            this._teacherService = teacherService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()

        {
            QueryResponse<Teacher> response = await _teacherService.GetAll();
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("Presences")]
        public async Task<IActionResult> Presence(int id)
        {
            QueryResponse<Presence> response = await _teacherService.GetPresenceListOfLesson(id);
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("Lessons")]
        public async Task<IActionResult> Lessons(int id)
        {
            QueryResponse<Lesson> response = await _teacherService.GetLessonsByTeacher(id);
           if (!response.Success)
           {
               return NotFound();
           }
           return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create(Teacher teacher)
        {
            SingleResponse<int> response = await _teacherService.Insert(teacher);
            return Ok(response);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Teacher teacher)
        {
            SingleResponse<Teacher> response = await _teacherService.GetTeacherByEmail(teacher.Email, teacher.Passcode);
            if (!response.Success)
            {
                return NotFound();
            }
            
            string token = TokenService.GenerateToken(response.Data.Email, "Teacher");
            User user = new User()
            {
                ID = response.Data.ID,
                Name = response.Data.TeacherName,
                Role = "Teacher",
                Token = token,
                UserName = response.Data.Email
            };

            return Ok(user);
        }
    }
}
