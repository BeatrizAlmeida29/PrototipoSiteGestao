using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace PrototipoSiteGestaoArtesBujujus.Servicos;

public static class Seguranca
{
    // Mesmo hash do banco: SHA2(senha, 256), em hexadecimal minúsculo
    public static string Sha256(string texto)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(texto));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }

    // "Funcionário" -> "funcionario"
    public static string Normalizar(string texto)
    {
        var decomposto = texto.Normalize(NormalizationForm.FormD);
        var sb = new StringBuilder();
        foreach (var c in decomposto)
        {
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                sb.Append(c);
        }
        return sb.ToString().ToLowerInvariant().Trim();
    }
}