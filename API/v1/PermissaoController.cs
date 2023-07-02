using api_csharp.API.exceptionhandler;
using AppClassLibraryClient.mapper;
using AppClassLibraryClient.model;
using AppClassLibraryDomain.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace api_csharp.API.v1
{
    [ApiController]
    [ApiVersion("1.0", Deprecated = true)]
    [Route("/v{version:apiVersion}/[controller]s")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PermissaoController : Controller
    {
        private PermissaoMapper permissaoMapper;
        private IPermissaoService _permissaoService;

        public PermissaoController(IPermissaoService permissaoService) : base()
        {
            permissaoMapper = new PermissaoMapper();
            _permissaoService = permissaoService;
        }

        /// <summary>
        /// Lista todas permissões cadastradas.
        /// </summary>
        /// <response code="200">Todas permissões.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet]
        [Authorize(Roles = "1")]
        [ProducesResponseType(typeof(IList<PermissaoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public IActionResult Todos()
        {
            try
            {
                return Ok(permissaoMapper.ToListResponse(_permissaoService.ListarTodos().ToList()));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
