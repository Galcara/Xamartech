using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdminAPIController : ControllerBase
    {
        private readonly IAdministratorService _adminService;
        private readonly ILessonService _lessonService;


        public AdminAPIController(IAdministratorService adminService, ILessonService lessonService)
        {
            this._adminService = adminService;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            QueryResponse<Administrator> response = await _adminService.GetAll();
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [Route("CreateLessons")]
        [HttpPost]
        public async Task<IActionResult> CreateLessons(List<Lesson> lessons)
        {
            if (lessons.Count == 0)
            {
                return BadRequest();
            }
            Response r = await _lessonService.InsertAllGeneratedLessons(lessons);
            return Ok(r);
        }


        [HttpPost]
        [Route("GenerateSchedule")]
        public async Task<IActionResult> GenerateSchedule(ScheduleRequest request)
        {
            QueryResponse<Lesson> response = await _lessonService.GenerateSchedule(request.InitialScheduleData);
            if (!response.Success)
            {
                return this.BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Administrator administrator)
        {
            SingleResponse<int> response = await _adminService.Insert(administrator);
            return Ok(response);
        }

        [Route("AdmLogin")]
        [HttpPost]
        public async Task<IActionResult> GetByEmail(Administrator adm)
        {
            SingleResponse<Administrator> response = await _adminService.GetAdmByEmail(adm.Email, adm.Passcode);
            if (!response.Success)
            {
                return NotFound();
            }

            string token = TokenService.GenerateToken(response.Data.Email, "Admin");
            User user = new User()
            {
                ID = response.Data.ID,
                Name = response.Data.AdmName,
                Role = "Teacher",
                Token = token,
                UserName = response.Data.Email
            };

            return Ok(user);

        }

    }
}
