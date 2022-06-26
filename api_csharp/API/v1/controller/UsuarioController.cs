using api_csharp.API.exceptionhandler;
using api_csharp.API.v1.mapper;
using api_csharp.API.v1.model;
using domain.DAO;
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
        private UsuarioDAO usuarioDAO;

        public UsuarioController() : base()
        {
            usuarioMapper = new UsuarioMapper();
            apiExceptionHandler = new ApiExceptionHandler();
            usuarioDAO = new UsuarioDAO();
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult ApagarPorId(int id)
        {
            var usuarioResponse = usuarioMapper.ToResponse(usuarioDAO.BuscarPorId(id));
            if (usuarioResponse != null)
            {
                usuarioDAO.ApagarPorId(id);
                return NoContent();
            }
            else
                return apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id));
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
        [ProducesResponseType(typeof(List<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public UsuarioResponse BuscarPorId(int id)
        {
            var usuarioResponse = usuarioMapper.ToResponse(usuarioDAO.BuscarPorId(id));
            if (usuarioResponse != null)
                return usuarioResponse;
            else
                throw new HttpRequestException(string.Format("Não foi encontrado usuário com Id: {0}", id), null, HttpStatusCode.NotFound);
            //return apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id));
        }

        /// <summary>
        /// Cadastra um usuário.
        /// </summary>
        /// <response code="201">Usuário cadastrado.</response>
        /// <response code="409">Já existe usuário cadastrado com o mesmo nome.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpPost]
        [Route("")]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<UsuarioResponse> Cadastrar([FromBody] UsuarioRequest usuarioRequest)
        {
            var usuario = usuarioDAO.BuscarPorNome(usuarioRequest.Nome);

            if (usuario == null)
            {
                usuario = usuarioMapper.ToModel(usuarioRequest);
                usuario.Ativo = true;
                usuario = usuarioDAO.Cadastrar(usuario);
                return Created(string.Empty, usuarioMapper.ToResponse(usuario));
            }
            else
                return apiExceptionHandler.GetProblema(
                    (int)HttpStatusCode.Conflict,
                    string.Format("Já existe usuário cadastrado com o Nome: {0}", usuarioRequest.Nome)
                    );

        }

        /// <summary>
        /// Lista todos usuários cadastrados.
        /// </summary>
        /// <response code="200">Todos usuários encontrados.</response>
        /// <response code="404">Não existe usuário cadastrado.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(List<UsuarioResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public List<UsuarioResponse> Todos()
        {
            return usuarioMapper.ToListResponse(usuarioDAO.Todos());
        }
    }
}
