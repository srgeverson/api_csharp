using api_csharp.API.exceptionhandler;
using api_csharp.API.v1.mapper;
using api_csharp.API.v1.model;
using domain.DAO;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp.API.v1.controller
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private UsuarioMapper usuarioMapper;
        private ApiExceptionHandler apiExceptionHandler;
        private UsuarioDAO usuarioDAO;

        public UsuarioController() : base()
        {
            usuarioMapper = new UsuarioMapper();
            apiExceptionHandler = new ApiExceptionHandler();
            usuarioDAO = new UsuarioDAO();
        }

        [HttpDelete]
        [Route("v1/clientes/{id?}")]
        public IActionResult ApagarPorId(int id)
        {
            try
            {
                var usuarioResponse = usuarioMapper.ToResponse(usuarioDAO.BuscarPorId(id));
                if (usuarioResponse != null)
                {
                    var usuarioApagado = usuarioDAO.ApagarPorId(id);
                    return NoContent();
                }
                else
                    return NotFound(apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, apiExceptionHandler.GetProblema(500, string.Format("Erro interno ocorreu: {0}", ex.Message)));
            }
        }

        [HttpGet]
        [Route("v1/clientes/{id?}")]
        public IActionResult BuscarPorId(int id)
        {
            try
            {
                var usuarioResponse = usuarioMapper.ToResponse(usuarioDAO.BuscarPorId(id));
                if (usuarioResponse != null)
                    return Ok(usuarioResponse);
                else
                    return NotFound(apiExceptionHandler.GetProblema(404, string.Format("Não foi encontrado usuário com Id: {0}", id)));
            }
            catch (Exception ex)
            {
                return StatusCode(500, apiExceptionHandler.GetProblema(500, string.Format("Erro interno ocorreu: {0}", ex.Message)));
            }
        }

        [HttpGet]
        [Route("v1/clientes")]
        public IActionResult Todos()
        {
            try
            {
                return Ok(usuarioMapper.ToListResponse(usuarioDAO.Todos()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, apiExceptionHandler.GetProblema(500, string.Format("Erro interno ocorreu: {0}", ex.Message)));
            }
        }
    }
}
