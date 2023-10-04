

using box_office.Models;
using box_office.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace box_office.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private AuthService _authService { get; }
    private ILogger<AuthController> _logger { get; }  
    public AuthController(ILogger<AuthController> logger, AuthService authService)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        try
        {
            string token = await _authService.Login(model);

            return Ok(token);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Registration([FromBody] RegistrationModel model)
    {
        try
        {
            string token = await _authService.Registrate(model);

            return Ok(token);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }


    [HttpPost("[action]")]
    public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
    {
        try
        {
            var userInfoClaim = User.Claims.FirstOrDefault(x => x.Type == "user_info");
            var userInfo = JsonConvert.DeserializeObject<AuthService.UserInfo>(userInfoClaim!.Value);
            var session = User.Claims.FirstOrDefault(x => x.Type == "session")?.Value;

            await _authService.LogOut(userInfo.Id, session.ToString());

            _logger.LogInformation($"user id={userInfo.Id} вышел из ситсемы");
            return Ok();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            return StatusCode(500, ex.Message);
        }
    }

}
