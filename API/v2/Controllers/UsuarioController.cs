using System.Data;
using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using api_csharp.API.v2.ExceptionHandlers;
using api_csharp.API.v2.Models;

namespace api_csharp.API.v2.Controllers;

[ApiController]
[ApiVersion("2.0", Deprecated = false)]
[Route("/v{version:apiVersion}/[controller]s")]
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
    /// <param name="logger">Instância de logs da aplicação</param>
    public UsuarioController(ILogger<UsuarioController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Lista todos usuários cadastrados.
    /// </summary>
    /// <response code="200">Todos usuários encontrados.</response>
    /// <response code="404">Não existe usuário cadastrado.</response>
    /// <response code="500">Erro interno de sistema.</response>
    [HttpGet, MapToApiVersion("2.0")]
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