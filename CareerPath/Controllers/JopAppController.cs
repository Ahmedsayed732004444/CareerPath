using CareerPath.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace CareerPath.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JopAppController(IJopAppServices _jopAppServices) : ControllerBase
    {
        [HttpGet("GetAllJopApp")]
        public async Task<IActionResult> GetAllJopAppAsync()
        {
            return Ok(await _jopAppServices.GetAllJopAppAsync());
        }
    }
}
