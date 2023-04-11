namespace api_csharp.API.v2.Models;
/// <summary>
/// Dados Básicos do Usuário
/// </summary>
public class UsuarioBasicoResponse
{
    /// <summary>
    /// Código de identificação do usuário
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Nome do usuário
    /// </summary>
    public string? Nome { get; set; }
}
