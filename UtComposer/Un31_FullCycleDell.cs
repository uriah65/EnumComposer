using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un31_FullCycleDell
    {
        private IEnumDbReader _dbReader;

        [TestInitialize()]
        public void Initialize()
        {
            _dbReader = new EnumSqlDbReader();
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void ParseFileAndDbDell()
        {
            if (ConstantsPR.IsDell == false)
            {
                /* this test is using live SQL server connection and will work only on specific machine.*/
                return;
            }

            string inputFile = @"..\..\Fake31_Dell.cs";
            string expectedFile = @"..\..\Fake31_Dell.txt";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            Assert.AreEqual(output, expected, "Output file should match expected file.");

            // do it second time
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output2 = File.ReadAllText(outputFile);
            Assert.AreEqual(output2, expected, "Second run of composer should not change the file.");
        }
    }
}