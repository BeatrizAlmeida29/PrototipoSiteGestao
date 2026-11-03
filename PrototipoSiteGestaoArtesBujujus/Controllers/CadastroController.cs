using Microsoft.AspNetCore.Mvc;

namespace PrototipoSiteGestaoArtesBujujus.Controllers;

public class CadastroController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }
}