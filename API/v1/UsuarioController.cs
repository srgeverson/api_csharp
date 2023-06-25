using api_csharp.API.exceptionhandler;
using api_csharp.API.v2.ExceptionHandlers;
using api_csharp.API.v2.Models;
using AppClassLibraryClient.mapper;
using AppClassLibraryClient.model;
using AppClassLibraryDomain.facade;
using AppClassLibraryDomain.model.DTO;
using AppClassLibraryDomain.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Reflection;

namespace api_csharp.API.v1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("/v{version:apiVersion}/[controller]s")]
    [Produces(MediaTypeNames.Application.Json)]
    /// <summary>
    /// Controlador de Usuario
    /// </summary>
    public class UsuarioController : ControllerBase
    {
        private static readonly string[] Summaries = new[] { "Geverson", "Jose", "Souza" };
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioService _usuarioService;
        private readonly IPermissaoService _permissaoService;
        private readonly IAuthorizationServerFacade _authorizationServerFacade;
        private ApiExceptionHandler _apiExceptionHandler;
        private UsuarioMapper _usuarioMapper;
        private ConfiguracaoTokenDTO _configuracaoTokenDTO;

        /// <summary>
        /// Construtor
        /// </summary>
        /// <param name="logger">Instância de logs da aplicação</param>
        /// <param name="usuarioService"></param>
        /// <param name="permissaoService"></param>
        /// <param name="authorizationServerFacade"></param>
        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioService usuarioService, IPermissaoService permissaoService, IAuthorizationServerFacade authorizationServerFacade)
        {
            _logger = logger;
            _usuarioService = usuarioService;
            _permissaoService = permissaoService;
            _apiExceptionHandler = new ApiExceptionHandler();
            _usuarioMapper = new UsuarioMapper();
            _authorizationServerFacade = authorizationServerFacade;
            _configuracaoTokenDTO = _authorizationServerFacade.ValidarConfigucaoDoToken(
                Environment.GetEnvironmentVariable("secret"),
                Environment.GetEnvironmentVariable("expired"),
                Environment.GetEnvironmentVariable("token"),
                Assembly.GetExecutingAssembly().GetName().Name
            );
        }

        /// <summary>
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet, MapToApiVersion("1.0")]
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


        /// <summary>
        /// Alterar um usuário por id.
        /// </summary>
        /// <response code="201">Usuário alterado.</response>
        /// <response code="404">Código do usuário não existe.</response>
        /// <response code="409">Já existe usuário cadastrado com o mesmo nome.</response>
        /// <response code="500">Erro interno de sistema.</response>
        /// <param name="id" example="37">Código do usuário a ser alterado.</param>
        [HttpPut]
        [Route("{id?}")]
        [Authorize(Roles = "1")]
        //[Authorize(Roles = "3,6")]
        [ProducesResponseType(typeof(UsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> Alterar(long id, [FromBody] UsuarioRequest usuarioRequest)
        {
            var usuario = _usuarioService.BuscarPorId(id);
            if (usuario == null)
                return _apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id));

            usuario = _usuarioService.BuscarPorNome(usuarioRequest.Nome);
            if (usuario != null)
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.Conflict, string.Format("Já existe usuário cadastrado com o Nome: {0}", usuarioRequest.Nome));

            usuario = _usuarioMapper.ToModel(usuarioRequest);
            _usuarioService.Alterar(usuario);
            //if (usuarioAtualizado)
            //    usuario = _usuarioService.BuscarPorId(id);

            return Ok(_usuarioMapper.ToResponse(usuario));

        }
    }
}
