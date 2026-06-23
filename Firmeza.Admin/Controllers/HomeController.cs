using Firmeza.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Admin.Controllers;

public class HomeController : Controller
{
    private readonly IDashboardService _dashboard;

    public HomeController(IDashboardService dashboard)
    {
        _dashboard = dashboard;
    }
    public async Task<IActionResult> Index()
    {
        var data = await _dashboard.Dashboard();
        return View(data);
    }
}