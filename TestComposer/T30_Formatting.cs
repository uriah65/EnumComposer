using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T30_Formatting
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
        public void NoMultipleFormatting()
        {
            string inputFile = @"..\..\T30\Input.cs";
            string outputFile = Path.GetTempFileName() + ".txt";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            composer.Compose(inputFile, outputFile, _dbReader);
            string s1 = File.ReadAllText(outputFile);

            // do it second time 
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // do it third time
            inputFile = outputFile;
            outputFile = outputFile + "3.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // do it forth time
            inputFile = outputFile;
            outputFile = outputFile + "4.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string s2 = File.ReadAllText(outputFile);

            Assert.AreEqual(s1, s2, "Multiple runs do not cause multiple reformatting.");
        }

        [TestMethod]
        public void Formatting()
        {
            string inputFile = @"..\..\T30\Input.cs";
            string expectedFile = @"..\..\T30\Output.cs";

            ComposerFiles composer = new ComposerFiles();

            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            ConstantsPR.AssertSpaceEqual(expected, output, "Output should have expected syntax.");
            ConstantsPR.AssertFormatEqual(expected, output, "Output should have expected format.");
        }

        [TestMethod]
        public void Formatting2()
        {
            string inputFile = @"..\..\T30\Input2.cs";
            string expectedFile = @"..\..\T30\Output2.cs";

            ComposerFiles composer = new ComposerFiles();

            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            ConstantsPR.AssertSpaceEqual(expected, output, "Output should have expected syntax.");
            ConstantsPR.AssertFormatEqual(expected, output, "Output should have expected format.");
        }



    }
}
