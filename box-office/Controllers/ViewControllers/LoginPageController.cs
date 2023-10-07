

using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

public class LoginPageController : Controller
{
    public IActionResult LoginPage()
    {
        return View();
    }
}
