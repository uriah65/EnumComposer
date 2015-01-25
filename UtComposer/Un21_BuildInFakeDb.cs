using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace UtComposer
{
    [TestClass]
    public class Un21_BuildInFakeDb
    {
        private IEnumDbReader _dbReader = null;

        [TestInitialize()]
        public void Initialize()
        {
            _dbReader = new EnumDbReader();
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void FakeBuildInDatabase()
        {
            string path = @"..\..\Fake21_CsFakeDb.cs";
            string sourceText = File.ReadAllText(path);

            ComposerStrings composer = new ComposerStrings(_dbReader);
            composer.Compose(sourceText);

            string txt = composer.GetResultFile();
            
            Assert.AreEqual(1, composer.EnumModels.Count);
            Assert.AreEqual(7, composer.EnumModels[0].Values.Count);

           
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void FakeBuildInDatabaseException()
        {
            string path = @"..\..\Fake21_CsFakeDb.cs";
            string sourceText = File.ReadAllText(path);
            sourceText = sourceText.Replace("T_Weekdays", "T_Wds");

            ComposerStrings composer = new ComposerStrings(new EnumDbReader());
            composer.Compose(sourceText);

            Assert.AreEqual(1, composer.EnumModels.Count);
            Assert.AreEqual(7, composer.EnumModels[0].Values.Count);

            string txt = composer.GetResultFile();
        }
    }
}