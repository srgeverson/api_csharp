namespace api_csharp.API.exceptionhandler
{
    public class ApiExceptionHandler : HttpRequestException
    {
        public static string MSG_ERRO_GENERICA_USUARIO_FINAL = "Ocorreu um erro interno inesperado no sistema. Tente novamente e se o problema persistir, entre em contato com o administrador do sistema.";

        private ApiExceptionHandler()
        {
        }

        public IEnumerable<Problema> getExcecao()
        {
            new ApiExceptionHandler();

            yield return new Problema()
            {
                Status = 400
            };
        }
    }
}
