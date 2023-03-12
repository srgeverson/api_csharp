using System.Data;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using api_csharp.API.Controllers.v2.Model;
using api_csharp.API.Controllers.v2.ExceptionHandlers;

namespace api_csharp.API.Controllers.v2;

[ApiController]
[ApiVersion("2.0")]
[Route("/v{version:apiVersion}/usuarios")]
[Produces(MediaTypeNames.Application.Json)]
/// <summary>
/// Controlador de Usuario
/// </summary>
public class UsuarioController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Geverson", "Jose", "Souza"
    };

    private readonly ILogger<UsuarioController> _logger;

    /// <summary>
    /// Construtor
    /// </summary>
    /// <param name="logger">Inst�ncia de logs da aplica��o</param>
    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Lista todos usu�rios cadastrados.
    /// </summary>
    /// <response code="200">Todos usu�rios encontrados.</response>
    /// <response code="404">N�o existe usu�rio cadastrado.</response>
    /// <response code="500">Erro interno de sistema.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IList<UsuarioBasicoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(IList<UsuarioBasicoResponse>), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status500InternalServerError)]
    public ActionResult<IList<UsuarioBasicoResponse>> GetUsuarios()
    {
        return Enumerable
            .Range(1, 3)
            .Select(index => new UsuarioBasicoResponse
            {
                Id = Random.Shared.Next(-20, 55),
                Nome = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToList();
    }
}