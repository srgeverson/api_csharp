using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Net;
using System.Net.Mime;

namespace api_csharp.API.exceptionhandler
{
    public class ApiExceptionHandler : ControllerBase, IMiddleware
    {
        public static string MSG_ERRO_GENERICA_USUARIO_FINAL = "Ocorreu um erro interno inesperado no sistema. Tente novamente e se o problema persistir, entre em contato com o administrador do sistema.";
        private Problema problema;

        public ApiExceptionHandler() : base()
        {
            problema = new Problema();
        }

        public ActionResult GetProblema(int status, string mensagem)
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

            return StatusCode(status, problema);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (HttpRequestException hrex)
            {
                await HandleHttpRequestExceptionAsync(context, hrex);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleHttpRequestExceptionAsync(HttpContext context, HttpRequestException httpRequestException)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problema = GetProblema(
                httpRequestException.StatusCode.HasValue ? Convert.ToInt32(httpRequestException.StatusCode.Value) : 500,
                httpRequestException.Message
                );

            return context.Response.WriteAsync(JsonConvert.SerializeObject(problema));
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var problema = GetProblema((int)HttpStatusCode.InternalServerError, ex.Message);

            return context.Response.WriteAsync(JsonConvert.SerializeObject(problema));
        }

        private string GetEnumDescription(Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = fieldInfo?.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
                return attributes.First().Description;

            return value.ToString();
        }
    }
}
