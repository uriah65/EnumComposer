using IEnumComposer;
using System.Data.SqlClient;

namespace EnumComposer
{
    public class EnumSqlDbReader : IEnumDbReader
    {
        private string _scnn;

        public EnumSqlDbReader(string scnn)
        {
            _scnn = scnn;
        }

        public EnumSqlDbReader(string sqlServer, string sqlDatabase)
        {
            _scnn = string.Format("Server={0};Database={1};Trusted_Connection=True;", sqlServer, sqlDatabase);
        }

        public void ReadEnumeration(EnumModel model)
        {
            using (SqlConnection cnn = new SqlConnection(_scnn))
            {
                SqlCommand cmd = new SqlCommand(model.Sql, cnn);
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int value = reader.GetInt32(0);
                        string name = reader.GetString(1);
                        model.FillFromDb(value, name);
                    }
                }
            }
        }
    }
}