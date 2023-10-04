

using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayController : AppControllerBase
{
    public PlayController(IServiceProvider serviceProvider, ILogger<PlayController> logger) :
        base(serviceProvider, logger)
    { }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        return Ok("этот метод можно использовать только по токену");
    }
}
