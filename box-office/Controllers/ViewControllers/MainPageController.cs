

using Microsoft.AspNetCore.Mvc;

namespace box_office.Controllers;

public class MainPageController : Controller
{
    public IActionResult AdminPage()
    {
        return View();
    }
}
