using Microsoft.AspNetCore.Mvc;
using PrototipoSiteGestaoArtesBujujus.Repositories;

namespace PrototipoSiteGestaoArtesBujujus.Controllers;

public class LoginController : Controller
{
    private readonly IUsuarioRepository _repositorio;

    public LoginController(IUsuarioRepository repositorio)
    {
        _repositorio = repositorio;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Index(string email, string senha)
    {
       
        {
            var usuario = _repositorio.BuscarPorEmail(email);

            if (usuario == null || usuario.Senha != senha)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            return RedirectToAction("Index", "Home");
        }


     
    }
}