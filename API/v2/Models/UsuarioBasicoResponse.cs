namespace api_csharp.API.v2.Models;
/// <summary>
/// Dados B�sicos do Usu�rio
/// </summary>
public class UsuarioBasicoResponse
{
    /// <summary>
    /// C�digo de identifica��o do usu�rio
    /// </summary>
    public int Id { get; set; }
    /// <summary>
    /// Nome do usu�rio
    /// </summary>
    public string? Nome { get; set; }
}
