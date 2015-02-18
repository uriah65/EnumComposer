using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T32_FullCycleODBC
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
        public void Reading_Csv_ODBC()
        {
            //string path = @"..\..\";
            string inputFile = @"..\..\T32\Input.cs";
            string expectedFile = @"..\..\T32\Output.cs";

            _dbReader = new DbReader(null, null, null);
            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            ConstantsPR.AssertSpaceEqual(expected, output, "Output should be as expected");
        }
    }
}