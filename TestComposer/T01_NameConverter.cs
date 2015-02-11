using EnumComposer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestComposer
{
    [TestClass]
    public class T01_NameConverter
    {
        [TestInitialize()]
        public void Initialize()
        {
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void LegalCsName()
        {
            /* different option names intentionally coming from data, should be coerced to C# legal names*/
            Assert.AreEqual("we", EnumNameConverter.MakeValidIdentifier(" w  e"));
            Assert.AreEqual("_2we", EnumNameConverter.MakeValidIdentifier("2w  e"));
            Assert.AreEqual("_2w_____e", EnumNameConverter.MakeValidIdentifier("2w#$%^&e"));
            Assert.AreEqual("empty", EnumNameConverter.MakeValidIdentifier("  "));
            Assert.AreEqual("_as", EnumNameConverter.MakeValidIdentifier("as"));
            Assert.AreEqual("_as", EnumNameConverter.MakeValidIdentifier("  a   s  "));
            Assert.AreEqual("_enum", EnumNameConverter.MakeValidIdentifier("enum"));
        }

    }
}
