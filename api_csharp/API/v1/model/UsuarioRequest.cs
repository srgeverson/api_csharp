using Swashbuckle.AspNetCore.Filters;
using System.ComponentModel.DataAnnotations;

namespace api_csharp.API.v1.model
{
    public class UsuarioRequest
    {
        /// <summary>
        /// Nome de usuário para acessar a conta
        /// </summary>
        /// <example>srgeverson</example>
        [Required]
        public string? Nome { get; set; }

        /// <summary>
        /// Senha do usuário
        /// </summary>
        /// <example>q1w2e3r4</example>
        [Required]
        public string? Senha { get; set; }
    }
}
