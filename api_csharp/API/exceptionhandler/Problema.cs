namespace api_csharp.API.exceptionhandler
{
    public class Problema
    {
        public int Status { get; set; }

        public DateTime DataHora { get; set; }

        public string? Type { get; set; }

        public string? Titulo { get; set; }

        public string? Detalhe { get; set; }

        public string? MensagemUsuario;
    }
}