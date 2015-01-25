using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IEnumComposer;
using EnumComposer;

namespace UtComposer
{
    /* Test auxiliary class that to convert arbitrary short text to a valid C# name for the enumeration option.
    */

    [TestClass]
    public class Un01_NameConverter
    {

        EnumNameConverter _converter = null;

        [TestInitialize()]
        public void Initialize()
        {
            _converter =  new EnumNameConverter();
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void TestMethod1()
        {
            Assert.AreEqual("we", EnumNameConverter.MakeValidIdentifier(" w  e"));
            Assert.AreEqual("_2we", EnumNameConverter.MakeValidIdentifier("2w  e"));
            Assert.AreEqual("_2w_____e", EnumNameConverter.MakeValidIdentifier("2w#$%^&e"));
            Assert.AreEqual("empty", EnumNameConverter.MakeValidIdentifier("  "));
            Assert.AreEqual("_as", EnumNameConverter.MakeValidIdentifier("as"));
            Assert.AreEqual("_as", EnumNameConverter.MakeValidIdentifier("  a   s  "));

            //string s = "";
            //s = EnumNameConverter.MakeValidIdentifier("as");
            //s = EnumNameConverter.MakeValidIdentifier("a    s");

        }

        /* replacing blanks with 'empty' 
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidArgument()
        {
            _converter.Convert(" ");
        }*/

    }
}
