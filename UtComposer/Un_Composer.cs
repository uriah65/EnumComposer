﻿using EnumComposer;
using IEnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace UtComposer
{
    /* Test composer. Hight level test.
    */

    [TestClass]
    public class Un_Composer
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

            string path = @"..\..\Fake_CsEnumerations.cs";
            ComposerStrings composer = new ComposerStrings(null);
            string sourceText = File.ReadAllText(path);

            composer.Compose(sourceText);

            Assert.AreEqual(3, composer.EnumModels.Count);

            EnumModel model = composer.EnumModels[1];
            Assert.AreEqual("E2", model.Name);
            Assert.AreEqual("E2", model.NameSc);
            Assert.AreEqual("SELECT lkc__id, lkc_name FROM AcClaim.dbo.T_LookupCategories", model.Sql);
            Assert.AreEqual(2, model.Values.Count);

            EnumModelValue value = null;
            value = model.Values[0];
            Assert.AreEqual(344, value.Value);
            Assert.AreEqual(null, value.Name);
            Assert.AreEqual("K2", value.NameCs);

            value = model.Values[1];
            Assert.AreEqual(534, value.Value);
            Assert.AreEqual(null, value.Name);
            Assert.AreEqual("K5", value.NameCs);
        }

        [TestMethod]
        public void ParseFileAndDb()
        {
            string path = @"..\..\Fake_CsEnumerations2.cs";
            ComposerStrings composer = new ComposerStrings(_dbReader);
            string sourceText = File.ReadAllText(path);

            composer.Compose(sourceText);

            string txt = composer.GetResultFile();// .ToString();

            /* this test is for manual checking of text result above */
            Assert.AreEqual(true, true);
        }
    }
}