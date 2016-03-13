using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumComposer;
using System.IO;
using IEnumComposer;

namespace TestComposer
{
    [TestClass]
    public class T40_NewAttribute
    {
        private IEnumDbReader _dbReader = null;

        [TestInitialize()]
        public void Initialize()
        {
            _dbReader = new DbReader(null, null, null);
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            string inputFile = @"..\..\T40\Input.cs";                       
            string sourceCode = File.ReadAllText(inputFile);

            CodeScanner scanner = new CodeScanner(_dbReader, null);
            scanner.Scan(sourceCode);


            //composer.Compose(inputFile, outputFile, _dbReader);
        }
    }
}
