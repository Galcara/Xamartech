using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;


namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SubjectAPIController : ControllerBase
    {
        private readonly ISubjectService _subjectService;

        public SubjectAPIController(ISubjectService subjectService)
        {
            this._subjectService = subjectService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            QueryResponse<Subject> response = await _subjectService.GetAll();
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Subject subject)
        {
            Response response = await _subjectService.Insert(subject);
            return Ok(response);
        }
    }
}
