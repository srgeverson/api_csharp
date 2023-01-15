using api_csharp.API.exceptionhandler;
using AppClassLibraryClient.mapper;
using AppClassLibraryClient.model;
using AppClassLibraryDomain.model;
using AppClassLibraryDomain.service;
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
        [Route("")]
        [Authorize]
        [ProducesResponseType(typeof(IList<PermissaoResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Problema), StatusCodes.Status500InternalServerError)]
        public ActionResult<IList<PermissaoResponse>> Todos()
        {
            return permissaoMapper.ToListResponse((List<Permissao>)_permissaoService.ListarTodos());
        }
    }
}
