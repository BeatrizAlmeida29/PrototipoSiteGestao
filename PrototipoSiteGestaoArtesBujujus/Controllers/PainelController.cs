using System.Globalization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrototipoSiteGestaoArtesBujujus.Data;
using PrototipoSiteGestaoArtesBujujus.Entidades;
using PrototipoSiteGestaoArtesBujujus.Models;
using PrototipoSiteGestaoArtesBujujus.Servicos;

namespace PrototipoSiteGestaoArtesBujujus.Controllers;

[Authorize]
public class PainelController : Controller
{
    private readonly AppDbContext _db;

    public PainelController(AppDbContext db)
    {
        _db = db;
    }

    // manda cada perfil pra sua área
    public IActionResult Index()
    {
        if (User.IsInRole("gerente")) return RedirectToAction("Gerencia");
        if (User.IsInRole("funcionario")) return RedirectToAction("Producao");
        return RedirectToAction("MeusPedidos");
    }

    [Authorize(Roles = "cliente")]
    public IActionResult MeusPedidos()
    {
        var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var modelo = Montar("Meus pedidos", "Acompanhe cada etapa dos seus pedidos.", false, usuarioId);
        return View("Painel", modelo);
    }

    [Authorize(Roles = "funcionario,gerente")]
    public IActionResult Producao()
    {
        var modelo = Montar("Fila de produção", "Pedidos em andamento, por prazo de entrega.", true, null, true);
        return View("Painel", modelo);
    }

    [Authorize(Roles = "gerente")]
    public IActionResult Gerencia()
    {
        var modelo = Montar("Painel do gerente", "Todos os pedidos da empresa.", true);
        modelo.PodeCadastrarFuncionario = true;
        return View("Painel", modelo);
    }

    private PainelViewModel Montar(string titulo, string subtitulo, bool mostrarCliente,
                                   int? usuarioId = null, bool somenteAbertos = false)
    {
        var modelo = new PainelViewModel
        {
            Titulo = titulo,
            Subtitulo = subtitulo,
            MostrarCliente = mostrarCliente
        };

        try
        {
            var consulta = from p in _db.Set<Pedido>()
                           join c in _db.Set<Cliente>() on p.ClienteId equals c.Id
                           select new { p, c };

            if (usuarioId != null)
                consulta = consulta.Where(x => x.c.UsuarioId == usuarioId);
            if (somenteAbertos)
                consulta = consulta.Where(x => x.p.StatusPedido != "Entregue");

            var dados = consulta
                .OrderBy(x => x.p.PrazoEntrega)
                .Select(x => new
                {
                    x.p.Id,
                    NomeCliente = x.c.Nome,
                    x.p.DataPedido,
                    x.p.PrazoEntrega,
                    x.p.StatusPedido,
                    x.p.ValorTotal
                })
                .ToList();

            var cultura = CultureInfo.GetCultureInfo("pt-BR");

            modelo.Pedidos = dados.Select(d => new PedidoLinha
            {
                Id = d.Id,
                Cliente = d.NomeCliente,
                Data = d.DataPedido.ToString("dd/MM/yyyy"),
                Prazo = d.PrazoEntrega.ToString("dd/MM/yyyy"),
                Status = d.StatusPedido,
                StatusClasse = "status-" + Seguranca.Normalizar(d.StatusPedido).Replace(' ', '-'),
                Valor = d.ValorTotal.ToString("C", cultura)
            }).ToList();
        }
        catch (Exception)
        {
            modelo.Erro = "Não foi possível carregar os pedidos. Verifique se o banco está ligado.";
        }

        return modelo;
    }
}