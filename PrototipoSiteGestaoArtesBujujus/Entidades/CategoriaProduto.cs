using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class CategoriaProduto
{
    public int Id { get; set; }

    public string NomeCategoria { get; set; } = null!;

    public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();
}
