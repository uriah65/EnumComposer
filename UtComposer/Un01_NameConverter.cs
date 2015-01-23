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
            // start with letter, or _
            // no spaces
            // contains only numbers and digits or _
            Assert.AreEqual("we", _converter.Convert(" w  e"));
            Assert.AreEqual("_2we", _converter.Convert("2w  e"));
            Assert.AreEqual("_2w_____e", _converter.Convert("2w#$%^&e"));
            Assert.AreEqual("empty", _converter.Convert("  "));

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
