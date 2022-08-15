using System.ComponentModel.DataAnnotations;

namespace api_csharp.API.v1.model
{
    public class PermissaoResponse
    {
        /// <summary>
        /// Identificador da permissão
        /// </summary>
        /// <example>srgeverson</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome da permissão
        /// </summary>
        /// <example>q1w2e3r4</example>
        public string? Nome { get; set; }

        /// <summary>
        /// Identifica se o usuário está ativo ou não
        /// </summary>
        /// <example>true</example>
        public bool Ativo { get; set; }

        /// <summary>
        /// Token de acesso aos recursos da API
        /// </summary>
        /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImFuZHJlYmFsdGllcmkiLCJTdG9yZSI6InVzZXIiLCJuYmYiOjE1NjE2MDI2NjYsImV4cCI6MTU2MjIwNzQ2NiwiaWF0IjoxNTYxNjAyNjY2fQ.y2cvZ02PLJG-BFPUAWn1fChTvS4rPOv\_iEEsLPd7eBg</example>
        public string? Token { get; set; }
}
}
