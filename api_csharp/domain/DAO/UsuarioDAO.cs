using api_csharp.domain.utils;
using domain.model;
using System.Data.SqlClient;
using System.Text;

namespace domain.DAO
{
    /// <summary>
    /// Classe responsável por consultar os registros relacionados à tabela usuarios.
    /// </summary>
    public class UsuarioDAO
    {
        public UsuarioDAO()
        {
        }

        public bool DeletePorId(int id)
        {
            try
            {
                var usuarioRemovido = false;
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();

                    var sqlCommand = new SqlCommand("DELETE FROM usuarios WHERE Id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    var sqlDataReader = sqlCommand.ExecuteReader();
                    usuarioRemovido = sqlDataReader.RecordsAffected > 0;

                    sqlConnection.Close();
                }
                return usuarioRemovido;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }

        public Usuario Insert(Usuario? usuario)
        {
            try
            {
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("INSERT INTO usuarios (Nome, Senha, Ativo) VALUES (@nome, @senha, @ativo);");
                    stringBuilder.Append("SELECT @@IDENTITY AS Id;");

                    var sqlCommand = new SqlCommand(stringBuilder.ToString(), sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@nome", usuario.Nome);
                    sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha);
                    sqlCommand.Parameters.AddWithValue("@ativo", usuario.Ativo);

                    var sqlDataReader = sqlCommand.ExecuteReader();

                    if (sqlDataReader.Read())
                    {
                        var resultSetToModel = new ResultSetToModel<Usuario>();
                        usuario = resultSetToModel.ToModel(sqlDataReader, usuario);
                    }

                    sqlConnection.Close();
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }

        public List<Usuario> Select()
        {
            try
            {
                var usuarios = new List<Usuario>();
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();
                    var sqlCommand = new SqlCommand("SELECT * FROM usuarios;", sqlConnection);
                    var sqlDataReader = sqlCommand.ExecuteReader();

                    var resultSetToModel = new ResultSetToModel<Usuario>();
                    usuarios = resultSetToModel.ToListModel(sqlDataReader);

                    sqlConnection.Close();
                }
                return usuarios;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }

        public Usuario? SelectPorId(int id)
        {
            try
            {
                Usuario? usuario = null;
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();

                    var sqlCommand = new SqlCommand("SELECT * FROM usuarios AS u WHERE u.Id = @id", sqlConnection);
                    sqlCommand.Parameters.AddWithValue("@id", id);

                    using (var sqlDataReader = sqlCommand.ExecuteReader())
                    {
                        if (sqlDataReader.Read())
                        {
                            var resultSetToModel = new ResultSetToModel<Usuario>();
                            usuario = resultSetToModel.ToModel(sqlDataReader, new Usuario());
                        }
                    }
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }

        public Usuario? SelectPorNome(string? nome)
        {
            try
            {
                Usuario? usuario = null;
                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();

                    using (var sqlCommand = new SqlCommand("SELECT * FROM usuarios AS u WHERE u.Nome = @nome;", sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@nome", nome);

                        var sqlDataReader = sqlCommand.ExecuteReader();
                        if (sqlDataReader.Read())
                        {
                            var resultSetToModel = new ResultSetToModel<Usuario>();
                            usuario = resultSetToModel.ToModel(sqlDataReader, new Usuario());
                        }
                    }
                }
                return usuario;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }

        public bool Update(Usuario? usuario)
        {
            try
            {
                var usuarioAtualizado = false;

                using (var sqlConnection = new SqlConnection(ConexaoDAO.URLCONEXAO))
                {
                    sqlConnection.Open();

                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("UPDATE usuarios ");
                    stringBuilder.Append("SET ");
                    stringBuilder.Append("Nome = ISNULL(@nome, Nome), ");
                    stringBuilder.Append("Senha = ISNULL(@senha, Senha), ");
                    stringBuilder.Append("Ativo = ISNULL(@ativo, Ativo) ");
                    stringBuilder.Append("WHERE Id = @id ");
                    using (var sqlCommand = new SqlCommand(stringBuilder.ToString(), sqlConnection))
                    {
                        sqlCommand.Parameters.AddWithValue("@nome", usuario.Nome.Equals(null) ? DBNull.Value : usuario.Nome);
                        sqlCommand.Parameters.AddWithValue("@senha", usuario.Senha.Equals(null) ? DBNull.Value : usuario.Senha);
                        sqlCommand.Parameters.AddWithValue("@ativo", usuario.Ativo.Equals(null) ? DBNull.Value : usuario.Ativo);
                        sqlCommand.Parameters.AddWithValue("@id", usuario.Id);

                        var sqlDataReader = sqlCommand.ExecuteReader();
                        usuarioAtualizado = sqlDataReader.RecordsAffected > 0;
                    }
                }
                return usuarioAtualizado;
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("Ocorreu um erro em {0}. Detalhes: {1}", this.GetType().Name, ex.Message));
            }
        }
    }
}
