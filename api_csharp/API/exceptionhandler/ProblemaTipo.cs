using System.ComponentModel;

namespace api_csharp.API.exceptionhandler
{
    public enum ProblemaTipo
    {
        [Description("Dados inválidos")]
        DADOS_INVALIDOS = 400,
        [Description("Acesso Negado")]
        ACESSO_NEGADO = 403,
        [Description("Erro de sistema")]
        ERRO_DE_SISTEMA = 503,
        //MethodArgumentTypeMismatchException
        [Description("Parâmetro inválido")]
        PARAMETRO_INVALIDO = 0,
        //Pode ser o mesmo do 400
        [Description("Mensagem incompreensível")]
        MENSAGEM_INCOMPREENSIVEL = 1,
        [Description("Recurso não encontrado")]
        RECURSO_NAO_ENCONTRADO = 404,
        [Description("Entidade em uso")]
        ENTIDADE_EM_USO = 409,
        [Description("Violação de regra de negócio")]
        ERRO_NEGOCIO = 2
    }
}
