using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un20_ComposerFiles
    {
        /* Test composer. Hight level test.
        */

        private IEnumDbReader _dbReader = null;

        [TestInitialize()]
        public void Initialize()
        {
            _dbReader = new Fake_DbReader();
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void ParseFileAndDb()
        {
            string inputFile = @"..\..\Fake_CsEnumerations2.cs";
            string outputFile = Path.GetTempFileName() + ".txt";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            composer.Compose(inputFile, outputFile, _dbReader);

            // do it second time
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // do it third time
            inputFile = outputFile;
            outputFile = outputFile + "3.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string s1 = File.ReadAllText(inputFile);
            string s2 = File.ReadAllText(outputFile);

            Assert.AreEqual(s1, s2, "Second run of composer should not change file");
        }

        [TestMethod]
        public void ParseFileAndDbDell()
        {
            if (ConstantsPR.IsDell == false)
            {
                /* this test is using live SQL server connection and will work only on specific machine.*/
                return;
            }

            string inputFile = @"..\..\Fake_CsEnumDellXPS.cs";
            string expectedFile = @"..\..\Fake_CsEnumDellXPSExpected.txt";

            ComposerFiles composer = new ComposerFiles();
            _dbReader = new EnumSqlDbReader();

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