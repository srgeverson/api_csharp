using domain.DAO;
using domain.model;
using Microsoft.AspNetCore.Mvc;

namespace api_csharp.API.v1.controller
{
    [Route("v1/clientes")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public List<Usuario> Todos()
        {
            try
            {
                var usuarioDAO = new UsuarioDAO();
                return usuarioDAO.Todos();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
