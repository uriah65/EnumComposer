using EnumComposer;
using System;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Uriah65.EnumComposerVSP
{
    public class ConfigReaderVsp : IEnumConfigReader
    {
        private string _configFilePath;

        public ConfigReaderVsp(EnvDTE.Project project)
        {
            _configFilePath = FindProjectConfigurationFile(project);
        }

        public static string FindProjectConfigurationFile(EnvDTE.Project project)
        {
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
            {
                if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
                {
                    return item.get_FileNames(0);
                }
            }

            return null;
        }

        public Tuple<string, string> GetConnectionString(string connectionStringName)
        {
            if (_configFilePath == null)
            {
                return null;
            }

            return ExtractConnectionString(_configFilePath, connectionStringName);
        }

        public Tuple<string, string> ExtractConnectionString(string configFilePath, string connectionStringName)
        {
            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
            configFileMap.ExeConfigFilename = configFilePath;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

            foreach (ConnectionStringSettings cnnNode in configuration.ConnectionStrings.ConnectionStrings)
            {
                if (cnnNode.Name.ToLowerInvariant() == connectionStringName.ToLowerInvariant())
                {
                    return new Tuple<string, string>(cnnNode.ProviderName, cnnNode.ConnectionString);
                }
            }

            return null;
        }
    }
}