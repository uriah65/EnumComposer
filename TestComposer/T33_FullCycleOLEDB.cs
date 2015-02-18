using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T33_FullCycleOLEDB
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
        public void Reading_Access_OleDB()
        {
            string inputFile = @"..\..\T33\Input.cs";
            string expectedFile = @"..\..\T33\Output.cs";

            _dbReader = new DbReader(null, null, null);
            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            ConstantsPR.AssertSpaceEqual(output, expected, "Output should be as expected.");
        }
    }
}