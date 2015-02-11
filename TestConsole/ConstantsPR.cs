using System.Diagnostics;

namespace TestConsole
{
    public static class ConstantsPR
    {
        // path to console executables
        private const string CONSOLE_EXE = @"EnumComposerConsole.exe";
        private const string CONSOLE_DEBUG_PATH = @"..\..\..\EnumComposerConsole\bin\Debug\" + CONSOLE_EXE;
        private const string CONSOLE_RELEASE_PATH = @"..\..\..\EnumComposerConsole\bin\Release\" + CONSOLE_EXE;
        private const string CONSOLE_PATH = CONSOLE_DEBUG_PATH;



        public static void RunVisible(string arguments, string exePath = CONSOLE_PATH)
        {
            Process p = new Process();
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.FileName = exePath;
            p.Start();
            p.WaitForExit();
        }

        public static string[] RunHidden(string arguments, string exePath = CONSOLE_PATH)
        {
            string[] result = new string[2];

            Process p = new Process();
            // Redirect the output stream of the child process.
            p.StartInfo.Arguments = arguments;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = exePath;
            p.Start();
            result[0] = p.StandardOutput.ReadToEnd();
            result[1] = p.StandardError.ReadToEnd();
            p.WaitForExit();

            return result;
        }
    }
}