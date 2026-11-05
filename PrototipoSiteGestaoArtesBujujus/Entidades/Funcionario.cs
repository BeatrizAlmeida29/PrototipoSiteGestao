using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class Funcionario
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }

    public string Nome { get; set; } = null!;

    public string Cpf { get; set; } = null!;

    public virtual Usuario Usuario { get; set; } = null!;
}
