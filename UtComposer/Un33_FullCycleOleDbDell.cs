using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IEnumComposer;
using EnumComposer;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un33_FullCycleOleDbDell
    {
        private IEnumDbReader _dbReader;

        [TestInitialize()]
        public void Initialize()
        {
            //_dbReader = new EnumDbReader();
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void TestMethod1()
        {
            string path = @"..\..\";
            string inputFile = @"..\..\Fake33_.cs";
            string expectedFile = @"..\..\Fake33_Expected.txt";

            _dbReader = new EnumDbReader();


            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);            
            string expected = File.ReadAllText(expectedFile);
            Assert.AreEqual(expected, output, "Output should be as expected");
        }
    }
}
