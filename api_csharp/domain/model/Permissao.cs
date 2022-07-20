namespace domain.model
{
    public class Permissao
    {
        private int? id;
        private string? nome;
        private string? descricao;
        private bool? ativo;

        public int? Id { get => id; set => id = value; }
        public string? Nome { get => nome; set => nome = value; }
        public string? Descricao { get => descricao; set => descricao = value; }
        public bool? Ativo { get => ativo; set => ativo = value; }
    }
}
