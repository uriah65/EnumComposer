using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace TestComposer
{
    [TestClass]
    public class T20_General
    {
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
        public void ParseFile()
        {
            /* Parse C# file and make sure EmunModel array get filled correctly */
            string path = @"..\..\T20\Input.cs";
            string sourceText = File.ReadAllText(path);

            ComposerStrings composer = new ComposerStrings(null);
            composer.Compose(sourceText);

            Assert.AreEqual(1, composer.EnumModels.Count);

            EnumModel model = composer.EnumModels[0];
            Assert.AreEqual("E2", model.Name);
            Assert.AreEqual("SELECT lkc__id, lkc_name FROM AcClaim.dbo.T_LookupCategories", model.SqlSelect);
            Assert.AreEqual(2, model.Values.Count);
            Assert.AreEqual("server1", model.SqlProvider);
            Assert.AreEqual("database2", model.SqlDatasource);

            EnumModelValue value = null;
            value = model.Values[0];
            Assert.AreEqual(344, value.Value);
            Assert.AreEqual("K2", value.NameCs);

            value = model.Values[1];
            Assert.AreEqual(534, value.Value);
            Assert.AreEqual("K5", value.NameCs);
        }
    }
}