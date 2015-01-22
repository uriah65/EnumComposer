using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer
{
    public class DedbugLog : IEnumLog
    {
        public void WriteLine(string format, params object[] arguments)
        {
            string message = string.Format("EnumComposer: " + format, arguments);
            System.Diagnostics.Debug.WriteLine(message);
        }
    }
}
