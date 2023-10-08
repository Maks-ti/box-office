


using box_office.Models;
using box_office.Services;
using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketController : AppControllerBase
{
    private TicketService TicketService { get; set; }
    public TicketController(IServiceProvider serviceProvider, ILogger<TicketController> logger, TicketService service) :
        base(serviceProvider, logger)
    { TicketService = service; }

    [HttpPost("[action]")]
    public async Task<IActionResult> SetSold([FromBody] TicketIsSoldUpdateModel model)
    {
        try
        {
            await TicketService.SetSold(model);

            return Ok();
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
