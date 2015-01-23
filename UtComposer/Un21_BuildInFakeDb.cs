using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    [TestClass]
    class Un21_BuildInFakeDb
    {
        private IEnumDbReader _dbReader = null;

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
        public void FakeBuildInDatabase()
        {
            string path = @"..\..\Fake_CsFakeDb.cs";
            string sourceText = File.ReadAllText(path);

            ComposerStrings composer = new ComposerStrings(_dbReader);
            composer.Compose(sourceText);

            Assert.AreEqual(1, composer.EnumModels.Count);
            Assert.AreEqual(7, composer.EnumModels[0].Values.Count);

            string txt = composer.GetResultFile();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FakeBuildInDatabaseException()
        {
            string path = @"..\..\Fake_CsFakeDb.cs";
            string sourceText = File.ReadAllText(path);
            sourceText = sourceText.Replace("T_Weekdays", "T_Wds");

            ComposerStrings composer = new ComposerStrings(new EnumSqlDbReader());
            composer.Compose(sourceText);

            Assert.AreEqual(1, composer.EnumModels.Count);
            Assert.AreEqual(7, composer.EnumModels[0].Values.Count);

            string txt = composer.GetResultFile();
        }
    }
}
