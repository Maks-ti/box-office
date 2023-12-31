﻿
using box_office.Models;
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

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var result = await HallService.GetAllAsync();

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
            var result = await HallService.GetByIdAsync(id);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create([FromBody] HallCreateModel model)
    {
        try
        {
            var result = await HallService.CreateAsync(model);

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
            await HallService.DeleteByIdAsync(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500);
        }
    }
}
