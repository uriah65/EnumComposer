using IEnumComposer;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace EnumComposer
{
    public class DbReader : IEnumDbReader
    {
        private IEnumLog _log;

        //public Func<string, Tuple<string, string>> _readConfigFunction = null;
        public IEnumConfigReader _configReader = null;

        private DbTypeEnum _dbType;
        private string _scnn;

        private const string DEFAULT_CONNECTION_NAME = "EnumComposer";
        private const string CONFIG_MARKER = "#CONFIG";
        private const string SQL_MARKER = "#SQL";
        private const string OLEDB_MARKER = "#OLEDB";
        private const string ODBC_MARKER = "#ODBC";
        private const string FAKE_MARKER = "#FAKE";

        /* a fake imitation of SQL server build in this class to ease e2e testing */
        private const string FAKE_SQL_SINATURE = "server=fakesqlserver;database=fakedb";

        public DbReader(string sqlServer, string sqlDatabase, IEnumLog log)
        {
            _log = log;
            if (_log == null)
            {
                _log = new DedbugLog();
            }

            _scnn = BuildSqlConnectionString(sqlServer, sqlDatabase);
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
            /* once new connection string provided, the consecutive EnumModels will be using it */
            bool newConnection = BuildConnectionString(model);
            if (newConnection)
            {
                _log.WriteLine("conn:\t {0}.", _scnn);
            }

            if (string.IsNullOrWhiteSpace(_scnn))
            {
                /* all attempts failed */
                throw new ApplicationException(string.Format("Connection string for the enumeration '{0}' is blank.", model.Name));
            }

            _log.WriteLine("enum:\t {0}\t\t {1}.", model.Name, model.SqlSelect);

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

        private bool BuildConnectionString(EnumModel model)
        {
            if (model.SqlProvider == CONFIG_MARKER)
            {
                /* if configuration file is specified */
                string connectionName = DEFAULT_CONNECTION_NAME;
                if (string.IsNullOrWhiteSpace(model.SqlDatasource) == false)
                {
                    connectionName = model.SqlDatasource;
                }
                _scnn = ReadConfig(connectionName);
                return true;
            }
            else if (string.IsNullOrWhiteSpace(model.SqlProvider) == false)
            {
                /* other wise another MARKERS are processed */
                _scnn = BuildSqlConnectionString(model.SqlProvider, model.SqlDatasource);
                return true;
            }

            if (string.IsNullOrWhiteSpace(_scnn) && _configReader != null)
            {
                /* if there is still no connection string, attempt to obtain default values from the configuration files */
                _scnn = ReadConfig(DEFAULT_CONNECTION_NAME);
                 return true;
            }

            return false;
        }

        private string BuildSqlConnectionString(string part1, string part2)
        {
            _dbType = DbTypeEnum.SqlServer;

            part1 = ("" + part1).Trim();
            part2 = ("" + part2).Trim();

            if (string.IsNullOrWhiteSpace(part1) || string.IsNullOrWhiteSpace(part2))
            {
                return "";
            }

            string scnn = "";
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

            bool useDescriptions = model.SqlSelect.ToLower().Contains("description");

            List<string> T_Weekdays = new List<string>
            {
                "Sunday",
                "Monday",
                "Tuesday",
                "Wednesday",
                "Thursday",
                "Friday",
                "Saturday",
            };

            List<string> descriptions = new List<string>
            {
                 "Old English Sunnandæg (pronounced [ˈsunnɑndæj), meaning \"sun's day.\"",
                 "Old English Mōnandæg (pronounced [ˈmoːnɑndæj]), meaning \"Moon's day.\"",
                 "Old English Tīwesdæg (pronounced [ˈtiːwezdæj], meaning \"Tiw's day.\"",
                 "Old English Wōdnesdæg (pronounced [ˈwoːdnezdæj) meaning the day of the Germanic god Wodan",
                 "Old English Þūnresdæg (pronounced [ˈθuːnrezdæj]), meaning 'Þunor's day'.",
                 "Old English Frīgedæg (pronounced [ˈfriːjedæj]), meaning the day of the Anglo-Saxon goddess Fríge",
                 "The only day of the week to retain its Roman origin in English, named after the Roman god Saturn",
            };

            for (int i = 0; i < 7; i++)
            {
                string desciption = useDescriptions ? descriptions[i] : null;
                model.FillFromDb(i, T_Weekdays[i], desciption);
            }
        }

        #endregion Fake Database

        #region Provider Parsing

        public static string ProviderNameParsing(string netProvoderName)
        {
            if (netProvoderName != null)
            {
                netProvoderName = netProvoderName.ToLowerInvariant();
                switch (netProvoderName)
                {
                    case "system.data.sqlclient":
                        return SQL_MARKER;

                    case "system.data.oledb":
                        return OLEDB_MARKER;

                    case "system.data.odbc":
                        return ODBC_MARKER;
                }
            }

            throw new ApplicationException("Provider '" + netProvoderName + "' is not supported by the EnumComposer");
        }

        #endregion Provider Parsing

        private string ReadConfig(string connectionName)
        {
            Tuple<string, string> values = _configReader.GetConnectionString(connectionName);
            if (values == null)
            {
                return null;
            }

            string provider = DbReader.ProviderNameParsing(values.Item1);

            return BuildSqlConnectionString(provider, values.Item2);
        }
    }
}