using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ClassAPIController : ControllerBase
    {
        private readonly IClassService _classService;

        public ClassAPIController(IClassService classService)
        {
            this._classService = classService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            QueryResponse<Class> response = await _classService.GetAll();
            if (!response.Success)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Class clasS)
        {
            Response response = await _classService.Insert(clasS);
            return Ok(response);
        }
    }
}
