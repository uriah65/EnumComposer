//using System;
//using System.Configuration;
//using System.Text.RegularExpressions;

//namespace Uriah65.EnumComposerVSP
//{
//    public class Helpers
//    {
//        private static Tuple<string, string> ReadConfigurationConnectionString(EnvDTE.Project project, string connectionStringName)
//        {
//            string configFilePath = FindProjectConfigurationFile(project);

//            if (configFilePath == null)
//            {
//                return null;
//            }

//            return ExtractConnectionString(configFilePath, connectionStringName);
//        }

//        public static string FindProjectConfigurationFile(EnvDTE.Project project)
//        {
//            foreach (EnvDTE.ProjectItem item in project.ProjectItems)
//            {
//                if (Regex.IsMatch(item.Name, "(app|web).config", RegexOptions.IgnoreCase))
//                {
//                    return item.get_FileNames(0);
//                }
//            }

//            return null;
//        }

//        public static Tuple<string, string> ExtractConnectionString(string configFilePath, string connectionStringName)
//        {
//            ExeConfigurationFileMap configFileMap = new ExeConfigurationFileMap();
//            configFileMap.ExeConfigFilename = configFilePath;
//            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(configFileMap, ConfigurationUserLevel.None);

//            foreach (ConnectionStringSettings cnnNode in configuration.ConnectionStrings.ConnectionStrings)
//            {
//                if (cnnNode.Name.ToLowerInvariant() == connectionStringName.ToLowerInvariant())
//                {
//                    return new Tuple<string, string>(cnnNode.ProviderName, cnnNode.ConnectionString);
//                }
//            }

//            return null;
//        }
//    }
//}