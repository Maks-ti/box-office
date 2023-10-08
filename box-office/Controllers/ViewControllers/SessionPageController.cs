
using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

public class SessionPageController : Controller
{
    public IActionResult SessionPage()
    {
        return View();
    }

    public IActionResult SessionDetail()
    {
        return View();
    }
}
