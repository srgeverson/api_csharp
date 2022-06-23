using System.ComponentModel;
using System.Reflection;

namespace api_csharp.API.exceptionhandler
{
    public class ApiExceptionHandler : HttpRequestException
    {
        public static string MSG_ERRO_GENERICA_USUARIO_FINAL = "Ocorreu um erro interno inesperado no sistema. Tente novamente e se o problema persistir, entre em contato com o administrador do sistema.";
        private Problema problema;

        public ApiExceptionHandler() : base()
        {
            problema = new Problema();
        }

        public Problema GetProblema(int status, string mensagem)
        {
            switch (status)
            {
                case (int)ProblemaTipo.RECURSO_NAO_ENCONTRADO:
                    problema.Status = (int)ProblemaTipo.RECURSO_NAO_ENCONTRADO;
                    problema.Titulo = GetEnumDescription(ProblemaTipo.RECURSO_NAO_ENCONTRADO);
                    problema.Type = ProblemaTipo.RECURSO_NAO_ENCONTRADO.GetType().Name;
                    break;
                case (int)ProblemaTipo.ENTIDADE_EM_USO:
                    problema.Status = (int)ProblemaTipo.ENTIDADE_EM_USO;
                    problema.Titulo = GetEnumDescription(ProblemaTipo.ENTIDADE_EM_USO);
                    problema.Type = ProblemaTipo.ENTIDADE_EM_USO.GetType().Name;
                    break;
                default:
                    problema.Status = (int)ProblemaTipo.ERRO_INTERNO;
                    problema.Titulo = GetEnumDescription(ProblemaTipo.ERRO_INTERNO);
                    problema.Type = ProblemaTipo.ERRO_INTERNO.GetType().Name;
                    break;
            }

            problema.Detalhe = mensagem;
            problema.DataHora = DateTime.Now;
            problema.MensagemUsuario = MSG_ERRO_GENERICA_USUARIO_FINAL;

            return problema;
        }

        private string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }
    }
}
