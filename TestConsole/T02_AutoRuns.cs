using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestConsole
{
    [TestClass]
    public class T02_AutoRuns
    {
        [TestInitialize()]
        public void Initialize()
        {
            /*
            var imageName = "EnumComposerConsole";
            var processes = Process.GetProcesses().Where(p => p.MainModule.FileName.StartsWith(imageName, true, CultureInfo.InvariantCulture));
            foreach (var proc in processes)
            {
                proc.Kill();
            }*/

            /*
            if (File.Exists(_exePath) == false)
            {
                throw new ApplicationException("File not found: " + _exePath + ".");
            }*/
        }

        [TestCleanup()]
        public void Cleanup()
        {
        }

        [TestMethod]
        public void Showing_HelpScreen()
        {
            string[] result = ConstantsPR.RunHidden("--help");

            string output = result[0];
            string errors = result[1];

            Assert.AreEqual(0, output.Length, "No output with --help argument");
            Assert.AreEqual(true, errors.Length > 980, "Help text shown with --help argument");
        }
    }
}