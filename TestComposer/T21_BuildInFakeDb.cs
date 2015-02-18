using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T21_BuildInFakeDb
    {
        private IEnumDbReader _dbReader = null;

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
        public void FakeBuildInDatabase()
        {
            string path = @"..\..\T21\Input.cs";
            string sourceText = File.ReadAllText(path);

            ComposerStrings composer = new ComposerStrings(_dbReader);
            composer.Compose(sourceText);

            string txt = composer.GetResultFile();

            Assert.AreEqual(2, composer.EnumModels.Count, "File contains two enumerations.");
            Assert.AreEqual(7, composer.EnumModels[0].Values.Count, "First enumeration has 7 values.");
            Assert.AreEqual(null, composer.EnumModels[0].Values[3].Description, "First enumeration has no descriptions.");
            Assert.AreEqual(7, composer.EnumModels[1].Values.Count, "Second enumeration has 7 values");
            Assert.AreEqual(2, composer.EnumModels[1].Values.Where(e => e.IsActive).Count(), "Second enumeration has 2 active values.");
            Assert.AreNotEqual(null, composer.EnumModels[1].Values[3].Description, "Second enumeration descriptions are filled up.");

        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void FakeBuildInDatabaseException()
        {
            string path = @"..\..\T21\Input.cs";
            string sourceText = File.ReadAllText(path);
            sourceText = sourceText.Replace("T_Weekdays", "T_Wds");

            ComposerStrings composer = new ComposerStrings(new DbReader(null, null, null));
            /* Build-in Database response only to T_Weekdays table name */
            composer.Compose(sourceText);
        }

    }
}
