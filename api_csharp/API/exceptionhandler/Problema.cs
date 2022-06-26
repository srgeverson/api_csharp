namespace api_csharp.API.exceptionhandler
{
    public class Problema
    {
        /// <summary>
        /// Códido do Status HTTP.
        /// </summary>
        /// <example>200</example>
        public int Status { get; set; }

        /// <summary>
        /// Data e hora do erro.
        /// </summary>
        /// <example>2022-06-25 15:15:34</example>
        public DateTime DataHora { get; set; }

        /// <summary>
        /// Tipo do erro.
        /// </summary>
        /// <example>Crítico</example>
        public string? Tipo { get; set; }

        /// <summary>
        /// Título do erro.
        /// </summary>
        /// <example>Dados inválidos</example>
        public string? Titulo { get; set; }

        /// <summary>
        /// Detalhes do erro.
        /// </summary>
        /// <example>Os dados informados não são aceitos</example>
        public string? Detalhe { get; set; }

        /// <summary>
        /// Detalhes do erro.
        /// </summary>
        /// <example>Ocorreu um erro interno inesperado no sistema. Tente novamente e se o problema persistir, entre em contato com o administrador do sistema.</example>
        public string? MensagemUsuario;
    }
}