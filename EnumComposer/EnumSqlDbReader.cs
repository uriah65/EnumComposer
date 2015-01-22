using IEnumComposer;
using System;
using System.Data.SqlClient;

namespace EnumComposer
{
    public class EnumSqlDbReader : IEnumDbReader
    {
        private string _scnn;

        public EnumSqlDbReader()
        {
        }

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
            try
            {
                ReadEnumeration_Inner(model);
            }
            catch (Exception exInner)
            {
                string message = string.Format("Error reading database for enumeration '{0}'. Connection string '{1}', select statement '{2}'.",
                   model.Name, _scnn, model.SqlSelect);
                throw new ApplicationException(message, exInner);
            }
        }

        public void ReadEnumeration_Inner(EnumModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SqlServer) == false)
            {
                /* once new database location provided, the consecutive models will be using it */
                _scnn = string.Format("Server={0};Database={1};Trusted_Connection=True;", model.SqlServer, model.SqlDatabase);
            }

            using (SqlConnection cnn = new SqlConnection(_scnn))
            {
                SqlCommand cmd = new SqlCommand(model.SqlSelect, cnn);
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