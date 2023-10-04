

using box_office.Services;
using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayController : AppControllerBase
{
    private PlayService PlayService { get; set; }
    public PlayController(IServiceProvider serviceProvider, ILogger<PlayController> logger, PlayService service) :
        base(serviceProvider, logger)
    { PlayService = service; }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        var result = await PlayService.GetAllAsync();

        return Ok(result);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        return Ok();
    }
}
