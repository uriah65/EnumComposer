using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using EnumComposer.Interfaces;
using EnumComposer;

namespace TestComposer
{
    [TestClass]
    public class T02_ConfigurationFiles
    {
        IEnumConfigFileLocator _reader;
        string[] _connection;

        [TestInitialize()]
        public void Initialize()
        {
            _reader = new ConfigReader();
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void Parsing_ConfigFile()
        {
            string inputFile = @"..\..\T02\App.config";
            string inputText = File.ReadAllText(inputFile);

            /* read valid connection */
            _connection = _reader.ExtractConnection("cnn1", inputText);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read valid connection, case invariant */
            _connection = _reader.ExtractConnection("cNN1", inputText);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read not valid connection */
            _connection = _reader.ExtractConnection("BAD_Cnn1", inputText);
            ConstantsPR.AssertConnectionString(null, "", _connection, "Expected equal.");
        }

        public void VisitingConfigFiles()
        {
            string startPath = @"..\..\T02\T02-1\T02-2";
            string endPath = @"..\..\T02";

            /* read connection from the T02 */
            _reader.GetConnection("cNn1", startPath, endPath);
            ConstantsPR.AssertConnectionString("System.Data.ProviderName", "Valid Connection String;", _connection, "Expected equal.");

            /* read connection from the T02-2 */
            _reader.GetConnection("ZzZ", startPath, endPath);
            ConstantsPR.AssertConnectionString("providerZZ", "cnnZZcnn", _connection, "Expected equal.");
        }
    }
}
