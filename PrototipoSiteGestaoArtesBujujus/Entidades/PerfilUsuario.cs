using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class PerfilUsuario
{
    public int Id { get; set; }

    public string NomePerfil { get; set; } = null!;

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
