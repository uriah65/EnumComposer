using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T31_FullCycleSQL
    {
        private IEnumDbReader _dbReader;

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
        public void SqlServer_NamePair()
        {
            if (ConstantsPR.HasNoAdventureWorks)
            {
                /* AdventureWorks sample database has to be available. */
                return;
            }

            string inputFile = @"..\..\T31\Input.cs";
            string expectedFile = @"..\..\T31\Output.cs";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            ConstantsPR.AssertSpaceEqual(expected, output, "Files should match.");
            //Assert.AreEqual(output, expected, "Output file should match expected file.");

            // do it second time
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output2 = File.ReadAllText(outputFile);
            ConstantsPR.AssertSpaceEqual(output2, expected, "Second run of composer should not change the file.");
            //Assert.AreEqual(output2, expected, "Second run of composer should not change the file.");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SqlServer_NamePairWrong()
        {
            if (ConstantsPR.HasNoAdventureWorks)
            {
                /* AdventureWorks sample database has to be available. */
                throw new ApplicationException();
            }

            string inputFile = @"..\..\T31\Input2.cs";
            //string expectedFile = @"..\..\Fake31_Dell.txt";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);
        }

        [TestMethod]
        public void SqlServer_ConnectionString()
        {
            if (ConstantsPR.HasNoAdventureWorks)
            {
                /* AdventureWorks sample database has to be available. */
                return;
            }

            string inputFile = @"..\..\T31\Input3.cs";
            string expectedFile = @"..\..\T31\Output.cs";

            ComposerFiles composer = new ComposerFiles();

            // do it first time
            string outputFile = Path.GetTempFileName() + ".txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output = File.ReadAllText(outputFile);
            string expected = File.ReadAllText(expectedFile);
            output = output.RemoveTextBetween("[EnumSqlCnn(", ")]");
            expected = expected.RemoveTextBetween("[EnumSqlCnn(", ")]");

            ConstantsPR.AssertSpaceEqual(expected, output, "Output file should match expected file.");

            // do it second time
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            // compare with expected
            string output2 = File.ReadAllText(outputFile);
            output2 = output2.RemoveTextBetween("[EnumSqlCnn(", ")]");
            ConstantsPR.AssertSpaceEqual(expected, output2, "Second run of composer should not change the file.");
        }

    }
}
