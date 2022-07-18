using System.Data.SqlClient;
using System.Reflection;

namespace api_csharp.domain.utils
{
    /// <summary>
    /// Classe responsável converter o ResultSet retornado do banco de dados para o Objeto especificado.
    /// </summary>
    public class ResultSetToModel<T>
    {
        public List<T> ToListModel(SqlDataReader sqlDataReader)
        {
            var models = new List<T>();

            while (sqlDataReader.Read())
                models.Add(ToModel(sqlDataReader));

            return models;
        }

        public T ToModel(SqlDataReader sqlDataReader, bool isOneObject = false)
        {
            if (isOneObject)
                while (sqlDataReader.Read())
                    return ToModel(sqlDataReader, (T?)Activator.CreateInstance(typeof(T)));
            else
                return ToModel(sqlDataReader, (T?)Activator.CreateInstance(typeof(T)));

            return default(T);
        }

        private T ToModel(SqlDataReader sqlDataReader, T? model)
        {
            var columns = Enumerable.Range(0, sqlDataReader.FieldCount).Select(sqlDataReader.GetName).ToList();
            Type type = typeof(T);
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var propertyInfo in propertyInfos)
            {
                try
                {
                    columns.ForEach(column =>
                    {
                        if (column.ToLower().Equals(propertyInfo.Name.ToLower()))
                        {
                            if (sqlDataReader[propertyInfo.Name] != null)
                            {
                                var typeValue = propertyInfo.GetMethod.ReturnType.FullName.ToString();

                                if (typeValue.Contains("System.Int16"))
                                    propertyInfo?.SetValue(model, Convert.ToInt16(sqlDataReader[propertyInfo.Name]));
                                else if (typeValue.Contains("System.Int32"))
                                    propertyInfo?.SetValue(model, Convert.ToInt32(sqlDataReader[propertyInfo.Name]));
                                else if (typeValue.Contains("System.Int64"))
                                    propertyInfo?.SetValue(model, Convert.ToInt64(sqlDataReader[propertyInfo.Name]));
                                else if (typeValue.Contains("System.Date") || typeValue.Contains("System.DateTime"))
                                    propertyInfo?.SetValue(model, Convert.ToDateTime(sqlDataReader[propertyInfo.Name]));
                                else if (typeValue.Contains("System.Boolean"))
                                    propertyInfo?.SetValue(model, Convert.ToBoolean(sqlDataReader[propertyInfo.Name]));
                                else
                                    propertyInfo?.SetValue(model, Convert.ToString(sqlDataReader[propertyInfo.Name]));
                            }
                        }
                    });
                }
                catch (Exception)
                {
                    //Quando não encontrar a coluna ignorar
                }
            }

            return model;
        }
    }
}
