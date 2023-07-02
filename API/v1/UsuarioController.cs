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

        /// <summary>
        ///Apaga um usuário por Id.
        /// </summary>
        /// <response code="204">Usuário apagado com sucesso.</response>
        /// <response code="404">Código do usuário não existe.</response>
        /// <response code="500">Erro interno de sistema.</response>
        /// <param name="id" example="123">Código do usuário a ser apagado.</param>
        [HttpDelete]
        [Route("{id?}")]
        //[Authorize(Roles = "5")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult ApagarPorId(long id)
        {
            var usuarioResponse = _usuarioMapper.ToResponse(_usuarioService.BuscarPorId(id));
            if (usuarioResponse != null)
            {
                _usuarioService.Excluir(id);
                return NoContent();
            }
            else
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, string.Format("Não foi encontrado usuário com Id: {0}", id));
        }

        /// <summary>
        /// Buscar um usuário por Id.
        /// </summary>
        /// <response code="200">Usuário consultado.</response>
        /// <response code="404">Código do usuário não existe.</response>
        /// <response code="500">Erro interno de sistema.</response>
        /// <param name="id" example="123">Código do usuário a ser consultado.</param>
        [HttpGet, MapToApiVersion("1.0")]
        [Route("{id?}")]
        [Authorize(Roles = "1")]
        //[Authorize(Roles = "2")]
        [ProducesResponseType(typeof(List<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> BuscarPorId(long id)
        {
            var usuarioResponse = _usuarioMapper.ToResponse(_usuarioService.BuscarPorId(id));
            if (usuarioResponse != null)
                return usuarioResponse;
            else
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, string.Format("Não foi encontrado usuário com Id: {0}", id));
        }

        /// <summary>
        /// Cadastra um usuário.
        /// </summary>
        /// <response code="201">Usuário cadastrado.</response>
        /// <response code="409">Já existe usuário cadastrado com o mesmo nome.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpPost, MapToApiVersion("1.0")]
        [Route("")]
        [Authorize(Roles = "4")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> Cadastrar([FromBody] UsuarioRequest usuarioRequest)
        {
            var usuario = _usuarioService.BuscarPorNome(usuarioRequest.Nome);

            if (usuario == null)
            {
                usuario = _usuarioMapper.ToModel(usuarioRequest);
                usuario.Ativo = true;
                _usuarioService.Adicionar(usuario);
                return Created(string.Empty, _usuarioMapper.ToResponse(usuario));
            }
            else
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.Conflict, string.Format("Já existe usuário cadastrado com o Nome: {0}", usuarioRequest.Nome));

        }

        /// <summary>
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet, MapToApiVersion("1.0")]
        [Authorize(Roles = "2")]
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
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet, MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(IList<UsuarioBasicoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(IList<UsuarioBasicoResponse>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemaExceptionHandler), StatusCodes.Status500InternalServerError)]
        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<UsuarioLoginResponse> Logar([FromBody] UsuarioLoginRequest usuarioLoginRequest)
        {
            var usuario = _usuarioService.BuscarPorEmail(usuarioLoginRequest.Email);

            if (usuario == null)
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, "Usuário não encontrado!");

            if (!_usuarioService.ValidarSenha(usuarioLoginRequest.Senha, usuario.Senha))
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.BadRequest, "Senha inválida!");

            if (usuario.Ativo == false)
                return _apiExceptionHandler.GetProblema((int)HttpStatusCode.BadRequest, "Usuário desativado!");

            long[] permissoesId = _authorizationServerFacade.PermissoesPorEmailESistema(usuario.Email, _configuracaoTokenDTO.App);
            var usuarioLoginResponse = _usuarioMapper.ToLoginResponse(usuario);
            usuarioLoginResponse.Token = _authorizationServerFacade.GerarTokenNetCore(usuario, _configuracaoTokenDTO, permissoesId);//authorizationServer.GenerateToken(usuario, permissoes);
            return usuarioLoginResponse;
        }

        /// <summary>
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet]
        [Route("")]
        [Authorize(Roles = "2")]
        [ProducesResponseType(typeof(IList<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<IList<UsuarioResponse>> Todos()
        {
            return (List<UsuarioResponse>)_usuarioMapper.ToListResponse(_usuarioService.ListarTodos());
        }
    }
}
