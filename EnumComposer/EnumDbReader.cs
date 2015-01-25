using IEnumComposer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EnumComposer
{
    public class EnumDbReader : IEnumDbReader
    {
        private DbTypeEnum _dbType;
        private string _scnn;

        private const string SQL_MARKER = "#SQL";
        private const string OLEDB_MARKER = "#OLEDB";
        private const string ODBC_MARKER = "#ODBC";
        private const string FAKE_MARKER = "#FAKE";

        /* a fake imitation of SQL server build in this class to ease e2e testing */
        private const string FAKE_SQL_SINATURE = "server=fakesqlserver;database=fakedb";

        public EnumDbReader()
        {
        }

        public EnumDbReader(string sqlServer, string sqlDatabase)
        {
            _scnn = BuildConnection(sqlServer, sqlDatabase);
        }

        public void ReadEnumeration(EnumModel model)
        {
            try
            {
                ReadEnumeration_Inner(model);
            }
            catch (Exception ex)
            {
                string message = string.Format("Error reading database for the enumeration '{1}'.{0}Connection string is '{2}'.{0}SELECT statement is '{3}'.",
                   Environment.NewLine, model.Name, _scnn, model.SqlSelect);
                throw new ApplicationException(message, ex);
            }
        }

        public void ReadEnumeration_Inner(EnumModel model)
        {
            if (string.IsNullOrWhiteSpace(model.SqlServer) == false)
            {
                /* once new database location provided, the consecutive EnumModels will be using it */
                _scnn = BuildConnection(model.SqlServer, model.SqlDatabase);
            }

            if (string.IsNullOrWhiteSpace(_scnn))
            {
                throw new ApplicationException(string.Format("Connection string for the enumeration '{0}' is blank.", model.Name));
            }

            switch (_dbType)
            {
                case DbTypeEnum.SqlServer:
                    ReadSqlServer(model);
                    break;

                case DbTypeEnum.Oledb:
                    ReadOledb(model);
                    break;

                case DbTypeEnum.Odbc:
                    ReadOdbc(model);
                    break;

                case DbTypeEnum.BuildInFake:
                    ReadFake(model);
                    break;
            }
        }

        private string BuildConnection(string part1, string part2)
        {
            _dbType = DbTypeEnum.SqlServer;
            string scnn;

            part1 = ("" + part1).Trim();
            part2 = ("" + part2).Trim();

            switch (part1.ToUpper())
            {
                case SQL_MARKER:
                    _dbType = DbTypeEnum.SqlServer;
                    scnn = part2;
                    break;
                case OLEDB_MARKER:
                    _dbType = DbTypeEnum.Oledb;
                    scnn = part2;
                    break;
                case ODBC_MARKER:
                    _dbType = DbTypeEnum.Odbc;
                    scnn = part2;
                    break;
                case FAKE_MARKER:
                    _dbType = DbTypeEnum.BuildInFake;
                    scnn = part2;
                    break;
                default:
                    scnn = string.Format("Server={0};Database={1};Trusted_Connection=True;", part1, part2);
                    break;
            }



            if (scnn.ToLower().Contains(FAKE_SQL_SINATURE))
            {
                _dbType = DbTypeEnum.BuildInFake;
            }

            return scnn;
        }


        private void Read(DbDataReader reader, EnumModel model)
        {
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    int value = reader.GetInt32(0);
                    string name = reader.GetString(1);
                    string description = null;
                    if (reader.FieldCount > 2)
                    {
                        description = "" + reader.GetString(2);
                    }
                    model.FillFromDb(value, name, description);
                }
            }
        }

        #region SqlServer

        private void ReadSqlServer(EnumModel model)
        {
            using (SqlConnection cnn = new SqlConnection(_scnn))
            {
                SqlCommand cmd = new SqlCommand(model.SqlSelect, cnn);
                cnn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                Read(reader, model);
              }
        }

        #endregion SqlServer

        #region OleDb

        private void ReadOledb(EnumModel model)
        {
            using (OleDbConnection cnn = new OleDbConnection(_scnn))
            {
                OleDbCommand cmd = new OleDbCommand(model.SqlSelect, cnn);
                cnn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                Read(reader, model);
            }
        }

        #endregion OleDb

        #region ODBC

        private void ReadOdbc(EnumModel model)
        {
            using (OdbcConnection cnn = new OdbcConnection(_scnn))
            {
                OdbcCommand cmd = new OdbcCommand(model.SqlSelect, cnn);
                cnn.Open();
                OdbcDataReader reader = cmd.ExecuteReader();
                Read(reader, model);
            }
        }

        #endregion ODBC

        #region Fake Database

        private void ReadFake(EnumModel model)
        {
            if (model.SqlSelect.ToLower().Contains("t_weekdays") == false)
            {
                throw new ApplicationException(string.Format("Error executing statement '{0}' against Fake database.", model.SqlSelect));
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

        #endregion Fake Database
    }
}