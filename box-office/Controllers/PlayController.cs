

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
        try
        {
            var result = await PlayService.GetAllAsync();

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
            var result = await PlayService.GetByIdAsync(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(IFormFile pictureFile, [FromQuery] string name, [FromQuery] string Description)
    {
        try
        {
            var result = await PlayService.CreateAsync(pictureFile, name, Description);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPut("[action]")]
    public async Task<IActionResult> Update(IFormFile pictureFile, [FromQuery] string name, [FromQuery] string Description)
    {
        try
        {
            var result = await PlayService.UpdateAsync(pictureFile, name, Description);

            return Ok(result);
        }
        catch(Exception ex)
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
            await PlayService.DeleteByIdAsync(id);

            return NoContent();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
