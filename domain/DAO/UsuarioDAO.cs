
using domain.model;
using System.Data.SqlClient;

namespace domain.DAO
{
    public class UsuarioDAO
    {
        public UsuarioDAO()
        {
        }

        public List<Usuario> Todos()
        {
            try
            {
                var usuarios = new List<Usuario>();
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand("SELECT * FROM usuarios", sqlConnection);
                    var sqlDataReader = sqlCommand.ExecuteReader();
                    while (sqlDataReader.Read())
                    {
                        usuarios.Add(new Usuario()
                        {
                            Id = Convert.ToInt32(sqlDataReader["Id"]),
                            Nome = sqlDataReader["Nome"].ToString(),
                            Senha = sqlDataReader["Senha"].ToString(),
                            Ativo = Convert.ToBoolean(sqlDataReader["Ativo"]),
                        });
                    }
                    sqlConnection.Close();
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
