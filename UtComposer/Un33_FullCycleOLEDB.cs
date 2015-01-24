using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IEnumComposer;
using EnumComposer;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un33_FullCycleOLEDB
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
        public void ReadingAcccessDB()
        {
            if (ConstantsPR.IsDell == false)
            {
                /* this test is using live SQL server connection and will work only on specific machine.*/
                return;
            }

            string path = @"..\..\";
            string inputFile = @"..\..\Fake33_.cs";
            string dataFile = @"..\..\AccessTest.accdb";
            string expectedFile = @"..\..\Fake33_Expected.txt";

            //[EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=C:\__GitHub\EnumComposer\EnumComposer\UtComposer\AccessTest.accdb;Persist Security Info=False")]
            _dbReader = new EnumDbReader("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + dataFile + ";Persist Security Info=False");
            
            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);            
            string expected = File.ReadAllText(expectedFile);
            Assert.AreEqual(expected, output, "Output should be as expected.");
        }
    }
}
