using Microsoft.AspNetCore.Mvc;

namespace Puush.Controllers;

public class HomeController : Controller
{
    [HttpGet("/")]
    public IActionResult Index() => View();
    
    [HttpGet("/faq")]
    public IActionResult Faq() => View();
}