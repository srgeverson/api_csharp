using api_csharp.API.exceptionhandler;
using api_csharp.API.v1.mapper;
using api_csharp.API.v1.model;
using api_csharp.core;
using api_csharp.domain.service;
using domain.DAO;
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
        private ApiExceptionHandler apiExceptionHandler;
        private UsuarioMapper usuarioMapper;
        private UsuarioService usuarioService;

        public UsuarioController() : base()
        {
            usuarioMapper = new UsuarioMapper();
            apiExceptionHandler = new ApiExceptionHandler();
            usuarioService = new UsuarioService();
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
        [Authorize(Roles = "employee,manager")]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> Alterar(int id, [FromBody] UsuarioRequest usuarioRequest)
        {
            var usuario = usuarioService.BuscarPorId(id);
            if (usuario == null)
                return apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id));

            usuario = usuarioService.BuscarPorNome(usuarioRequest.Nome);
            if (usuario != null)
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.Conflict, string.Format("Já existe usuário cadastrado com o Nome: {0}", usuarioRequest.Nome));

            usuario = usuarioMapper.ToModel(usuarioRequest);
            var usuarioAtualizado = usuarioService.AlterarPorId(usuario, id);
            if (usuarioAtualizado)
                usuario = usuarioService.BuscarPorId(id);

            return Ok(usuarioMapper.ToResponse(usuario));

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
        [Authorize(Roles = "manager")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult ApagarPorId(int id)
        {
            var usuarioResponse = usuarioMapper.ToResponse(usuarioService.BuscarPorId(id));
            if (usuarioResponse != null)
            {
                usuarioService.ApagarPorId(id);
                return NoContent();
            }
            else
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, string.Format("Não foi encontrado usuário com Id: {0}", id));
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
        [Authorize(Roles = "employee")]
        [ProducesResponseType(typeof(List<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> BuscarPorId(int id)
        {
            var usuarioResponse = usuarioMapper.ToResponse(usuarioService.BuscarPorId(id));
            if (usuarioResponse != null)
                return usuarioResponse;
            else
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, string.Format("Não foi encontrado usuário com Id: {0}", id));
        }

        /// <summary>
        /// Cadastra um usuário.
        /// </summary>
        /// <response code="201">Usuário cadastrado.</response>
        /// <response code="409">Já existe usuário cadastrado com o mesmo nome.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee,manager")]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> Cadastrar([FromBody] UsuarioRequest usuarioRequest)
        {
            var usuario = usuarioService.BuscarPorNome(usuarioRequest.Nome);

            if (usuario == null)
            {
                usuario = usuarioMapper.ToModel(usuarioRequest);
                usuario.Ativo = true;
                usuario = usuarioService.Cadastrar(usuario);
                return Created(string.Empty, usuarioMapper.ToResponse(usuario));
            }
            else
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.Conflict, string.Format("Já existe usuário cadastrado com o Nome: {0}", usuarioRequest.Nome));

        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public ActionResult<UsuarioLoginResponse> Logar([FromBody] UsuarioLoginRequest usuarioLoginRequest)
        {
            // Recupera o usuário
            var usuario = usuarioService.BuscarPorNome(usuarioLoginRequest.Nome);

            // Verifica se o usuário existe
            if (usuario == null)
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.NotFound, string.Format("Usuário não encontrado!"));

            if (!usuario.Senha.Equals(usuarioLoginRequest.Senha))
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.BadRequest, string.Format("Senha inválida!"));

            if (usuario.Ativo == false)
                return apiExceptionHandler.GetProblema((int)HttpStatusCode.BadRequest, string.Format("Usuário desativado!"));

            // Gera o Token
            var authorizationServer = new AuthorizationServer();

            // Retorna os dados
            var user = usuarioMapper.ToLoginResponse(usuario);
            user.Token = authorizationServer.GenerateToken(usuario, new string[2] { "employee", "manager" });
            return user;
        }

        /// <summary>
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(typeof(List<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<UsuarioResponse>> Todos()
        {
            return usuarioMapper.ToListResponse(usuarioService.Todos());
        }
    }
}
