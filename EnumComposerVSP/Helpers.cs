using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;

namespace Uriah65.EnumComposerVSP
{
    public class Helpers
    {
        private string ReadConfigurationConnectionString(EnvDTE.Project project, string connectionName)
        {
            string configurationFile = null;
           
            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
            {
                if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
                {
                    configurationFile = item.get_FileNames(0);
                    break;
                }
            }


            if (string.IsNullOrEmpty(configurationFile) == false)
            {
                ExeConfigurationFileMap configFile = new ExeConfigurationFileMap();
                configFile.ExeConfigFilename = configurationFile;
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFile, ConfigurationUserLevel.None);

                foreach (ConnectionStringSettings cnnString in configuration.ConnectionStrings.ConnectionStrings)
                {
                    if (cnnString.Name.ToLowerInvariant() == connectionName.ToLowerInvariant())
                    {
                        return cnnString.ConnectionString;
                    }
                }
            }

            return null;
        }
    }
}