using System;
using System.Collections.Generic;

namespace PrototipoSiteGestaoArtesBujujus.Entidades;

public partial class Pedido
{
    public int Id { get; set; }

    public int ClienteId { get; set; }

    public DateOnly DataPedido { get; set; }

    public DateOnly PrazoEntrega { get; set; }

    public string StatusPedido { get; set; } = null!;

    public decimal ValorTotal { get; set; }

    public virtual Cliente Cliente { get; set; } = null!;

    public virtual ICollection<ItemPedido> ItemPedidos { get; set; } = new List<ItemPedido>();
}
