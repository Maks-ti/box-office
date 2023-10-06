
using box_office.Services;
using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

[Route("api/[controller]")]
[ApiController]
public class HallController : AppControllerBase
{
    private HallService HallService { get; set; }
    public HallController(IServiceProvider serviceProvider, ILogger<PlayController> logger, HallService hallService)
        : base(serviceProvider, logger)
    {
        HallService = hallService;
    }
}
