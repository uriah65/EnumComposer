using EnumComposer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposerConsole
{
    class Log : IEnumLog
    {
        public void WriteLine(string format, params object[] arguments)
        {
            string message = string.Format("EnumComposer: " +  format, arguments);
            Console.WriteLine(message);
            Debug.WriteLine(message);
        }
    }
}
