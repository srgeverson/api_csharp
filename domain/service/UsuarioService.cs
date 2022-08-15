using domain.DAO;
using domain.model;

namespace api_csharp.domain.service
{
    /// <summary>
    /// Classe responsável por tratar dos dados cadastrais relacionados usuários.
    /// </summary>
    public class UsuarioService
    {

        private UsuarioDAO usuarioDAO;

        public UsuarioService()
        {
            this.usuarioDAO = new UsuarioDAO();
        }

        public bool AlterarPorId(Usuario? usuario, int id)
        {
            usuario.Id = id;
            return usuarioDAO.Update(usuario);
        }

        public bool ApagarPorId(Int32 id)
        {
            return usuarioDAO.DeletePorId(id);
        }
        public Usuario? BuscarPorId(Int32 id)
        {
            return usuarioDAO.SelectPorId(id);
        }

        public Usuario? BuscarPorNome(string nome)
        {
            return usuarioDAO.SelectPorNome(nome);
        }

        public Usuario? Cadastrar(Usuario usuario)
        {
            usuario.Ativo = true;
            return usuarioDAO.Insert(usuario);
        }

        public List<Usuario> Todos()
        {
            return usuarioDAO.Select();
        }
    }
}
