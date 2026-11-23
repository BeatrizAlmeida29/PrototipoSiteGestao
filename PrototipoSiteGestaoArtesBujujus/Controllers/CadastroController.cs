using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PrototipoSiteGestaoArtesBujujus.Data;
using PrototipoSiteGestaoArtesBujujus.Entidades;
using PrototipoSiteGestaoArtesBujujus.Models;
using PrototipoSiteGestaoArtesBujujus.Servicos;

namespace PrototipoSiteGestaoArtesBujujus.Controllers;

public class CadastroController : Controller
{
    private readonly AppDbContext _db;

    public CadastroController(AppDbContext db)
    {
        _db = db;
    }

    // Cadastro público: SEMPRE cria cliente
    [HttpGet]
    public IActionResult Index() => View(new CadastroViewModel());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(CadastroViewModel m) => Salvar(m, paraFuncionario: false);

    // Cadastro de funcionário: SÓ o gerente
    [HttpGet]
    [Authorize(Roles = "gerente")]
    public IActionResult NovoFuncionario() => View("Index", new CadastroViewModel { ParaFuncionario = true });

    [HttpPost]
    [Authorize(Roles = "gerente")]
    [ValidateAntiForgeryToken]
    public IActionResult NovoFuncionario(CadastroViewModel m) => Salvar(m, paraFuncionario: true);

    private IActionResult Salvar(CadastroViewModel m, bool paraFuncionario)
    {
        // o tipo de cadastro é decidido AQUI, nunca pelo formulário
        m.ParaFuncionario = paraFuncionario;
        m.Nome = (m.Nome ?? "").Trim();
        m.Email = (m.Email ?? "").Trim();
        m.Cpf = new string((m.Cpf ?? "").Where(char.IsDigit).ToArray());
        m.Telefone = new string((m.Telefone ?? "").Where(char.IsDigit).ToArray());

        var senha = m.Senha ?? "";
        var confirmacao = m.ConfirmacaoSenha ?? "";
        m.Senha = "";
        m.ConfirmacaoSenha = "";

        var erros = new List<string>();

        if (m.Nome.Length < 3 || m.Nome.Length > 100)
            erros.Add("Informe o nome completo (até 100 caracteres).");
        if (m.Cpf.Length != 11)
            erros.Add("O CPF precisa ter 11 números.");
        if (!paraFuncionario && (m.Telefone.Length < 10 || m.Telefone.Length > 11))
            erros.Add("Informe o telefone com DDD (10 ou 11 números).");
        if (m.Email.Length > 50 || !new EmailAddressAttribute().IsValid(m.Email))
            erros.Add("Informe um e-mail válido (até 50 caracteres).");
        if (senha.Length < 6)
            erros.Add("A senha precisa ter pelo menos 6 caracteres.");
        if (senha != confirmacao)
            erros.Add("A confirmação de senha não confere.");

        try
        {
            if (erros.Count == 0)
            {
                if (_db.Set<Usuario>().Any(u => u.Email == m.Email))
                    erros.Add("Este e-mail já está cadastrado.");

                var cpfExiste = paraFuncionario
                    ? _db.Set<Funcionario>().Any(f => f.Cpf == m.Cpf)
                    : _db.Set<Cliente>().Any(c => c.Cpf == m.Cpf);
                if (cpfExiste)
                    erros.Add("Este CPF já está cadastrado.");
            }

            if (erros.Count == 0)
            {
                var papel = paraFuncionario ? "funcionario" : "cliente";
                var perfil = _db.Set<PerfilUsuario>()
                    .AsEnumerable()
                    .FirstOrDefault(p => Seguranca.Normalizar(p.NomePerfil) == papel);

                if (perfil == null)
                {
                    erros.Add("Perfil não encontrado no banco de dados.");
                }
                else
                {
                    using var transacao = _db.Database.BeginTransaction();

                    var usuario = new Usuario
                    {
                        PerfilUsuarioId = perfil.Id,
                        Email = m.Email,
                        Senha = Seguranca.Sha256(senha),
                        StatusConta = true
                    };
                    _db.Add(usuario);
                    _db.SaveChanges();

                    if (paraFuncionario)
                        _db.Add(new Funcionario { UsuarioId = usuario.Id, Nome = m.Nome, Cpf = m.Cpf });
                    else
                        _db.Add(new Cliente { UsuarioId = usuario.Id, Nome = m.Nome, Cpf = m.Cpf, Telefone = m.Telefone });

                    _db.SaveChanges();
                    transacao.Commit();

                    TempData["Sucesso"] = paraFuncionario
                        ? "Funcionário cadastrado com sucesso."
                        : "Conta criada! Faça login para entrar.";

                    return paraFuncionario
                        ? RedirectToAction("Gerencia", "Painel")
                        : RedirectToAction("Index", "Login");
                }
            }
        }
        catch (DbUpdateException)
        {
            erros.Add("Não foi possível salvar: e-mail ou CPF já cadastrado.");
        }
        catch (Exception)
        {
            erros.Add("Não foi possível acessar o banco de dados. Verifique se ele está ligado.");
        }

        ViewBag.Erros = erros;
        return View("Index", m);
    }
}