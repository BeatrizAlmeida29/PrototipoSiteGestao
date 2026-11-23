namespace PrototipoSiteGestaoArtesBujujus.Models;

public class PainelViewModel
{
    public string Titulo { get; set; } = "";
    public string Subtitulo { get; set; } = "";
    public bool MostrarCliente { get; set; }
    public bool PodeCadastrarFuncionario { get; set; }
    public string? Erro { get; set; }
    public List<PedidoLinha> Pedidos { get; set; } = new();
}

public class PedidoLinha
{
    public int Id { get; set; }
    public string Cliente { get; set; } = "";
    public string Data { get; set; } = "";
    public string Prazo { get; set; } = "";
    public string Status { get; set; } = "";
    public string StatusClasse { get; set; } = "";
    public string Valor { get; set; } = "";
}