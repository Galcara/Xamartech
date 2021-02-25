using BusinessLogicalLayer.Interfaces;
using Common;
using Metadata;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace XamarTechWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PresenceAPIController : ControllerBase
    {
        private readonly IPresenceService _presenceService;

        public PresenceAPIController(IPresenceService presenceService)
        {
            this._presenceService = presenceService;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateFrequency(string register)
        {
            Response response = await _presenceService.ApplyFrequency(register);
            if (!response.Success)
            {
                return BadRequest();
            }
            return Ok(response);
        }

    }
}
