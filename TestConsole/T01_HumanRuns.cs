using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConsole
{
    [TestClass]
    public class T01_HumanRuns
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
        public void Show_HelpScreen()
        {
            /* if proper arguments are missing the help screen will show */
            ConstantsPR.RunVisible(" -v");
        }
    }
}