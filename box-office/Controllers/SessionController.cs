

using box_office.Models;
using box_office.Services;
using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SessionController : AppControllerBase
{
    private SessionService SessionService { get; set; }
    public SessionController(IServiceProvider serviceProvider, ILogger<PlayController> logger, SessionService service) :
        base(serviceProvider, logger)
    { SessionService = service; }

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await SessionService.GetAll();

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Get([FromQuery] int id)
    {
        try
        {
            var result = await SessionService.GetByIdAsync(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] SessionCreateModel model)
    {
        try
        {
            var result = await SessionService.CreateAsync(model);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpDelete("[action]")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await SessionService.DeleteByIdAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
