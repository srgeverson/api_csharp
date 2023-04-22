﻿using api_csharp.API.v2.ExceptionHandlers;
using api_csharp.Helper;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace api_csharp.API.v2.Controllers
{
    /// <summary>
    /// Ducumentos disponíveis na aplicação
    /// </summary>
    [ApiController]
    [ApiVersion("2.0", Deprecated = false)]
    [Route("/v{version:apiVersion}/[controller]s")]
    [Produces(MediaTypeNames.Application.Pdf)]
    public class DocumentoController : Controller
    {
        private readonly ILogger<UsuarioController> _logger;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger">Instância de logs da aplicação</param>
        public DocumentoController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        #region Códigos de documentação
        /// <summary>
        /// Mostra o diagrama ULM da aplicação <see cref="DocumentoController"/>.
        /// </summary>
        /// <response code="200">Diagrama de classe da aplicação.</response>
        /// <response code="404">Não encontrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<ProblemaExceptionHandler>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status500InternalServerError)]
        #endregion
        [HttpGet("diagrama-uml")]
        [MapToApiVersion("2.0")]
        public IActionResult GetDiagramaUML()
        {
            try
            {
                return Ok(FileHelper.GetFile(Path.Combine(AppContext.BaseDirectory, "Docs", "Docs_UML.pdf")));
            }
            catch (Exception ex)
            {
                return StatusCode(
                    (int)HttpStatusCode.InternalServerError,
                    new ProblemaExceptionHandler()
                    {
                        StatusCode = 500,
                        Message = "Houve um erro interno, tente novamente se o problema persistir entre em contato com o administrador do sistema",
                        UserDatails = ex.Message
                    }
                    );
            }
        }
    }
}
