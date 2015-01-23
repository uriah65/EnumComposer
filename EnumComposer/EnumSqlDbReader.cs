using IEnumComposer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace EnumComposer
{
    public class EnumSqlDbReader : IEnumDbReader
    {
        private string _scnn;

        /* we have a fake imitation of SQL server build in to ease e2e testing */
        private const string FAKE_SQL_SINATURE = "server=fakesqlserver;database=fakedb";
        private bool _isFakeServer = false;


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
                string message = string.Format("Error reading database for the enumeration '{1}'.{0}Connection string is '{2}'.{0}SELECT statement is '{3}'.",
                   Environment.NewLine, model.Name, _scnn, model.SqlSelect);
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

            if (string.IsNullOrWhiteSpace(_scnn))
            {
                throw new ApplicationException(string.Format("Empty connection string for the enumeration '{0}'.", model.Name));
            }

            if (_scnn.ToLower().Contains(FAKE_SQL_SINATURE))
            {
                /* build in fake mini-database for e2e testing*/
                ProcessFake(model);
                return;
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

        private void ProcessFake(EnumModel model)
        {
            if (model.SqlSelect.ToLower().Contains("t_weekdays") == false)
            {
                throw new ApplicationException(string.Format("Error executing sql '{0}' against Fake database.", model.SqlSelect));
            }

            Dictionary<int, string> T_Weekdays = new Dictionary<int, string>
            {
                [1] = "Sunday",
                [2] = "Monday",
                [3] = "Tuesday",
                [4] = "Vacation",
                [5] = "Wednesday",
                [6] = "Thursday",
                [7] = "Friday",
            };

            foreach (var entry in T_Weekdays)
            {
                model.FillFromDb(entry.Key, entry.Value);
            }
        }

    }
}