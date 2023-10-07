

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace box_office.Controllers;

public class MainPageController : Controller
{
    public IActionResult MainPage()
    {
        return View();
    }
}
