using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T30_FullCycle
    {
        /* Test composer. Hight level test.
        */

        private IEnumDbReader _dbReader;

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
            string inputFile = @"..\..\T30\Input.cs";
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
        public void ParseFileAndText()
        {
            string path = @"..\..\T30\Input.cs";
            ComposerStrings composer = new ComposerStrings(_dbReader);
            string sourceText = File.ReadAllText(path);

            composer.Compose(sourceText);

            string txt = composer.GetResultFile();// .ToString();

            /* this test is for manual checking of text result above */
            Assert.AreEqual(true, true);
        }



    }
}
