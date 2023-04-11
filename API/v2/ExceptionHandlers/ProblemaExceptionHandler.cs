namespace api_csharp.API.v2.ExceptionHandlers
{
    /// <summary>
    /// Representação de Erro encontrato
    /// </summary>
    public class ProblemaExceptionHandler
    {
        /// <summary>
        /// Código de status da requisição
        /// </summary>
        public int? StatusCode { get; set; }
        /// <summary>
        /// Mensagem retornada
        /// </summary>
        public string? Message { get; set; }
        /// <summary>
        /// Mensagem detalhada
        /// </summary>
        public string? UserDatails { get; set; }
    }
}
