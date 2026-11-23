using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PrototipoSiteGestaoArtesBujujus.Data;
using PrototipoSiteGestaoArtesBujujus.Entidades;
using PrototipoSiteGestaoArtesBujujus.Servicos;

namespace PrototipoSiteGestaoArtesBujujus.Controllers;

public class LoginController : Controller
{
    private readonly AppDbContext _db;

    public LoginController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Index()
    {
        if (User.Identity?.IsAuthenticated == true)
            return RedirectToAction("Index", "Painel");

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(string email, string senha)
    {
        email = (email ?? "").Trim();
        senha = senha ?? "";

        Usuario? usuario;
        string papel;
        string? nome;

        try
        {
            usuario = _db.Set<Usuario>().FirstOrDefault(u => u.Email == email);

            if (usuario == null || !usuario.StatusConta || usuario.Senha != Seguranca.Sha256(senha))
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            var perfil = _db.Set<PerfilUsuario>().First(p => p.Id == usuario.PerfilUsuarioId);
            papel = Seguranca.Normalizar(perfil.NomePerfil);

            nome = papel == "cliente"
                ? _db.Set<Cliente>().Where(c => c.UsuarioId == usuario.Id).Select(c => c.Nome).FirstOrDefault()
                : _db.Set<Funcionario>().Where(f => f.UsuarioId == usuario.Id).Select(f => f.Nome).FirstOrDefault();
        }
        catch (Exception)
        {
            ViewBag.Erro = "Não foi possível acessar o banco de dados. Verifique se ele está ligado.";
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, usuario!.Id.ToString()),
            new(ClaimTypes.Name, nome ?? usuario.Email),
            new(ClaimTypes.Email, usuario.Email),
            new(ClaimTypes.Role, papel)
        };

        var identidade = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identidade));

        return RedirectToAction("Index", "Painel");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Sair()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/");
    }
}