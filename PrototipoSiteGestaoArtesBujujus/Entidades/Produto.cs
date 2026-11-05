using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class Produto
{
    public int Id { get; set; }

    public int CategoriaProdutoId { get; set; }

    public string NomeProduto { get; set; } = null!;

    public string Material { get; set; } = null!;

    public string Dimensao { get; set; } = null!;

    public decimal? Preco { get; set; }

    public virtual CategoriaProduto CategoriaProduto { get; set; } = null!;

    public virtual ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();
}
