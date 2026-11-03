using PrototipoSiteGestaoArtesBujujus.Models;

namespace PrototipoSiteGestaoArtesBujujus.Repositories;

public interface IUsuarioRepository
{
    Usuario? BuscarPorEmail(string email);
}