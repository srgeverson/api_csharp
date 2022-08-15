using api_csharp.API.exceptionhandler;
using api_csharp.API.v1.mapper;
using api_csharp.API.v1.model;
using api_csharp.domain.service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace api_csharp.API.v1.controller
{
    [ApiController]
    [Route("v1/permissoes")]
    [Produces(MediaTypeNames.Application.Json)]
    public class PermissaoController : ControllerBase
    {
        private PermissaoMapper permissaoMapper;
        private PermissaoService permissaoService;

        public PermissaoController() : base()
        {
            permissaoMapper = new PermissaoMapper();
            permissaoService = new PermissaoService();
        }

        /// <summary>
        /// Lista todas permissões cadastradas.
        /// </summary>
        /// <response code="200">Todas permissões.</response>
        /// <response code="500">Erro interno de sistema.</response>
        [HttpGet]
        [Route("")]
        [Authorize]
        [ProducesResponseType(typeof(List<PermissaoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<List<PermissaoResponse>> Todos()
        {
            return permissaoMapper.ToListResponse(permissaoService.Todos());
        }
    }
}
