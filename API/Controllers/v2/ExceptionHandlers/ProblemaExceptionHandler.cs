namespace api_csharp.API.Controllers.v2.ExceptionHandlers
{
    /// <summary>
    /// Representação de Erro encontrato
    /// </summary>
    public class ProblemaExceptionHandler
    {
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public string? UserDatails { get; set; }
    }
}
