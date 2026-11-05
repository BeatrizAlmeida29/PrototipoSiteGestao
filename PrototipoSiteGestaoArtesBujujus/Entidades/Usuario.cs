using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class Usuario
{
    public int Id { get; set; }

    public int PerfilUsuarioId { get; set; }

    public string Email { get; set; } = null!;

    public string Senha { get; set; } = null!;

    public bool StatusConta { get; set; }

    public virtual Cliente? Cliente { get; set; }

    public virtual Funcionario? Funcionario { get; set; }

    public virtual PerfilUsuario PerfilUsuario { get; set; } = null!;
}
