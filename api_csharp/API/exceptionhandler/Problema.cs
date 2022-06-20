namespace api_csharp.API.exceptionhandler
{
    public class Problema
    {
        private int status;

        private DateTime timestamp;

        private string? type;

        private string? title;

        private string? detail;

        private string? userMessage;

        public int Status { get => status; set => status = value; }
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }
        public string? Type { get => type; set => type = value; }
        public string? Title { get => title; set => title = value; }
        public string? Detail { get => detail; set => detail = value; }
        public string? UserMessage { get => userMessage; set => userMessage = value; }

        //private List<Object> objects;

        //public static class Object
        //{

        //	private string? nome;

        //	private string? mensagemUsuario;

        //}
    }
}