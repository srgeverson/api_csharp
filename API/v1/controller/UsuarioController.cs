using api_csharp.API.exceptionhandler;
using api_csharp.core;
using AppClassLibraryClient.mapper;
using AppClassLibraryClient.model;
using AppClassLibraryDomain.facade;
using AppClassLibraryDomain.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;

namespace api_csharp.API.v1.controller
{
    [ApiController]
    [Route("v1/usuarios")]
    [Produces(MediaTypeNames.Application.Json)]
    public class UsuarioController : ControllerBase
    {
        private ApiExceptionHandler _apiExceptionHandler;
        private UsuarioMapper _usuarioMapper;
        private IUsuarioService _usuarioService;
        private IPermissaoService _permissaoService;
        private IAuthorizationServerFacade _authorizationServerFacade;

        public UsuarioController(IUsuarioService usuarioService, IPermissaoService permissaoService, IAuthorizationServerFacade authorizationServerFacade) : base()
        {
            _usuarioService = usuarioService;
            _permissaoService = permissaoService;
            _apiExceptionHandler = new ApiExceptionHandler();
            _usuarioMapper = new UsuarioMapper();
            _authorizationServerFacade = authorizationServerFacade;
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
        [Authorize(Roles = "MANAGER,EMPLOYEE")]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
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
        [Authorize(Roles = "MANAGER")]
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
        [HttpGet]
        [Route("{id?}")]
        [Authorize(Roles = "listar_usuario")]
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
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "cadastrar_usuario")]
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

            var permissoes = _permissaoService.ListarTodos().Select(permisaoNome => permisaoNome.Nome).ToArray();
            var authorizationServer = new AuthorizationServer();

            var usuarioLoginResponse = _usuarioMapper.ToLoginResponse(usuario);
            usuarioLoginResponse.Token = authorizationServer.GenerateToken(usuario, permissoes);
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
        [Authorize(Roles = "1,2")]
        [ProducesResponseType(typeof(IList<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<IList<UsuarioResponse>> Todos()
        {
            return (List<UsuarioResponse>)_usuarioMapper.ToListResponse(_usuarioService.ListarTodos());
        }
    }
}
