using EnumComposer;
using System;
using System.Diagnostics;

namespace EnumComposerConsole
{
    internal class Log : IEnumLog
    {
        public void WriteLine(string format, params object[] arguments)
        {
            string message = string.Format("{0} EnumComposer: ", DateTime.Now.ToString("HH:mm:ss"));
            message += string.Format(format, arguments);
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}