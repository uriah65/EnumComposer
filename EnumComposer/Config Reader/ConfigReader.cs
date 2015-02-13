using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EnumComposer
{
    public class ConfigReader
    {
        string _fromBottomDirectory;
        string _toUpDirectory;
        IEnumLog _log;

        public ConfigReader(string fromBottomDirectory, string toUpDirectory, IEnumLog log)
        {
            _fromBottomDirectory = fromBottomDirectory;
            _toUpDirectory = toUpDirectory;
            _log = log;
        }

        public string[] LocateConnectionInVSP(string connectionName) //todo: too tight coupling here
        {
            try
            {
                string[] values = GetConnection(connectionName, _fromBottomDirectory, _toUpDirectory);
                if (values == null)
                {
                    return null;
                }

                if (values.Length != 2)
                {
                    throw new ApplicationException("Error"); //todo: text
                }

                string provider = DbReader.ProviderNameParsing(values[0]);
                string scnn = values[1];

                return new string[] { provider, scnn };
            }
            catch (Exception ex)
            {
                string message = DedbugLog.ExceptionMessage(ex); // todo: funny dependancy
                _log.WriteLine(message);
            }

            return null;
        }

        public string[] GetConnection(string connectionName, string fromBottomDirectory, string upToDirectory)
        {
            DirectoryInfo bottom = new DirectoryInfo(fromBottomDirectory);
            DirectoryInfo top = new DirectoryInfo(upToDirectory);

            /* check directories are inside each other. Requires as a defense and recursive stop point. */
            if (IsInside(bottom.FullName, top.FullName) == false)
            {
                throw new ApplicationException("Invalid path order. From '" + bottom.FullName + "' to '" + top.FullName + "'.");
            }

            string[] cnn = GetConnection_Inner(connectionName, bottom, top.FullName);

            return cnn;
        }

        private string[] GetConnection_Inner(string connectionName, DirectoryInfo dirInfo, string topFullName)
        {
            List<FileInfo> fileInfos = dirInfo.GetFiles("App.config").ToList();
            fileInfos.AddRange(dirInfo.GetFiles("Web.config"));

            foreach (FileInfo fileInfo in fileInfos)
            {
                string text = File.ReadAllText(fileInfo.FullName);
                string[] cnn = ExtractConnection(connectionName, text);
                if (cnn != null)
                {
                    return cnn;
                }
            }

            if (dirInfo.FullName == topFullName)
            {
                /* top has been reached, cnn not found */
                return null;
            }

            dirInfo = dirInfo.Parent;
            if (dirInfo == null)
            {
                /* absolute top */
                return null;
            }

            /* dive up */
            return GetConnection_Inner(connectionName, dirInfo, topFullName);
        }

        public string[] ExtractConnection(string connectionName, string configFileText)
        {
            if (string.IsNullOrWhiteSpace(connectionName) || string.IsNullOrWhiteSpace(configFileText))
            {
                return null;
            }

            XDocument doc = XDocument.Parse(configFileText);
            XElement section = doc.Descendants("connectionStrings").SingleOrDefault();
            if (section == null)
            {
                return null;
            }

            List<XElement> scnns = section.Elements("add").ToList();
            connectionName = connectionName.ToUpperInvariant();
            XElement cnn = scnns.Where(e => e.Attribute("name").Value.ToUpperInvariant() == connectionName).SingleOrDefault();
            if (cnn == null)
            {
                return null;
            }

            string provider = cnn.Attribute("providerName").Value;
            string connection = cnn.Attribute("connectionString").Value;
            return new string[] { provider, connection };
        }

        public bool IsInside(string childPath, string parentPath)
        {
            if (childPath == null || parentPath == null)
            {
                return false;
            }

            childPath = childPath.ToUpperInvariant();
            parentPath = parentPath.ToUpperInvariant();
            if (parentPath.EndsWith(@"\") == false)
            {
                parentPath += @"\";
            }

            if (childPath == parentPath)
            {
                return true;
            }

            if (childPath.Length <= parentPath.Length)
            {
                /* child must be longer */
                return false;
            }

            if (parentPath != childPath.Substring(0, parentPath.Length))
            {
                /* both must be the same as parent goes */
                return false;
            }

            if (childPath[parentPath.Length - 1] == '\\')
            {
                /* at first differ symbol \ should be  */
                return true;
            }

            return false;
        }
    }
}