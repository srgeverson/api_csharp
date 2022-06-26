namespace api_csharp.API.v1.model
{
    public class UsuarioResponse
    {
        /// <summary>
        /// Nome de login do usuário
        /// </summary>
        /// <example>srgeverson</example>
        public int Id { get; set; }

        /// <summary>
        /// Nome de login do usuário
        /// </summary>
        /// <example>q1w2e3r4</example>
        public string? Nome { get; set; }

        /// <summary>
        /// Identifica se o usuário está ativo ou não
        /// </summary>
        /// <example>true</example>
        public bool Ativo { get; set; }
    }
}
