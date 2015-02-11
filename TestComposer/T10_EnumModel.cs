using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using EnumComposer;
using IEnumComposer;

namespace TestComposer
{
    /* Test Enumeration Model class
        */

    [TestClass]
    public class Un10_EnumModel
    {
        EnumModel _model;
        IEnumDbReader _dbReader;

        [TestInitialize()]
        public void Initialize()
        {
            _model = new EnumModel();
            _dbReader = new Fake_DbReader();

            _model.SqlSelect = "SELECT id, name FROM T_Simple";
            _model.Name = "A Test Enumeration";
        }

        [TestCleanup()]
        public void Cleanup()
        {

        }

        [TestMethod]
        public void AddingValuesAndDbReaderAndToString()
        {
            _model.FillFromCode(5, "inDb1", true);
            _model.FillFromCode(700, "notInDb1", true);
            _model.FillFromCode(6, "inDb2", false);
            _model.FillFromCode(701, "notInDb2", false);

            _dbReader.ReadEnumeration(_model);


            string actualCs = _model.ToCSharp();
            actualCs = actualCs.Replace(Environment.NewLine, "");
            actualCs = actualCs.Replace("\t", "");
            actualCs = actualCs.Replace(" ", "");

            string expectedCs = "inDb1=5,//inDb2=6,//inDbNew=7,";
            Assert.AreEqual(expectedCs, actualCs);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionOnValueTwice()
        {
            _model.FillFromCode(2, "aaa", true);
            _model.FillFromCode(2, "bbb", true);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExceptionOnNameTwice()
        {
            _model.FillFromCode(2, "aaa", true);
            _model.FillFromCode(3, "aaa", false);
        }
    }
}
