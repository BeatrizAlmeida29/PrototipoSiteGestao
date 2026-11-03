using PrototipoSiteGestaoArtesBujujus.Models;

namespace PrototipoSiteGestaoArtesBujujus.Repositories;

public class UsuarioRepositoryFake : IUsuarioRepository
{
    private readonly List<Usuario> _usuarios = new()
    {
        new Usuario {Id = 1, Nome = "Beatriz", Email = "beatriz@email.com", Senha = "b123", TipoUsuario = "gerente"},

        new Usuario {Id = 2, Nome = "David", Email = "david@email.com", Senha = "d123", TipoUsuario = "funcionario"},

        new Usuario {Id = 3, Nome = "Ariane", Email = "ariane@email.com", Senha = "a123", TipoUsuario = "cliente"}

    };


    public Usuario? BuscarPorEmail(string email)
    {
        return _usuarios.FirstOrDefault(us => us.Email == email); 
    }
}