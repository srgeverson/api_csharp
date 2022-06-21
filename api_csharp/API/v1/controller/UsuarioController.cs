using api_csharp.API.v1.mapper;
using api_csharp.API.v1.model;
using domain.DAO;
using domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp.API.v1.controller
{
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        [HttpDelete]
        [Route("v1/clientes/{id?}")]
        public void ApagarPorId(int id)
        {
            try
            {
                var usuarioDAO = new UsuarioDAO();
                usuarioDAO.ApagarPorId(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("v1/clientes/{id?}")]
        public UsuarioResponse PorId(int id)
        {
            try
            {
                var usuarioMapper = new UsuarioMapper();
                var usuarioDAO = new UsuarioDAO();
                return usuarioMapper.ToResponse(usuarioDAO.PorId(id));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet]
        [Route("v1/clientes")]
        public List<UsuarioResponse> Todos()
        {
            try
            {
                var usuarioMapper = new UsuarioMapper();
                var usuarioDAO = new UsuarioDAO();
                return usuarioMapper.ToListResponse(usuarioDAO.Todos());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
