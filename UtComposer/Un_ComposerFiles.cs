using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un_ComposerFiles
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


            //ProcessStartInfo info1 = new ProcessStartInfo(inputFile);
            //Process.Start(info1);

            //ProcessStartInfo info2 = new ProcessStartInfo(outputFile);
            //Process.Start(info2);

            string s1 = File.ReadAllText(inputFile);
            string s2 = File.ReadAllText(outputFile);

            Assert.AreEqual(s1, s2, "Second run of composer should not change file");

            //ProcessStartInfo info = new ProcessStartInfo(outputFile);
            //Process.Start(info);
        }

        [TestMethod]
        public void ParseFileAndDbDell()
        {
            if (ConstantsPR.IsDell == false)
            {
                return;
            }

            string inputFile = @"..\..\Fake_CsEnumDellXPS.cs";
            string outputFile = Path.GetTempFileName() + ".txt";

            ComposerFiles composer = new ComposerFiles();

            //_dbReader = new EnumSqlDbReader("DELLSTUDIOXPS", "Arctics");
            _dbReader = new EnumSqlDbReader("", "");

            // do it first time
            composer.Compose(inputFile, outputFile, _dbReader);

            // do it second time
            inputFile = outputFile;
            outputFile = outputFile + "2.txt";
            composer.Compose(inputFile, outputFile, _dbReader);

            string s1 = File.ReadAllText(inputFile);
            string s2 = File.ReadAllText(outputFile);
            string s3 = File.ReadAllText(@"..\..\Fake_CsEnumDellXPSExpected.txt");

            Assert.AreEqual(s1, s2, "Second run of composer should not change the file.");
            Assert.AreEqual(s1, s3, "Output file should match expected file.");

            //ProcessStartInfo info = new ProcessStartInfo(outputFile);
            //Process.Start(info);
        }
    }
}