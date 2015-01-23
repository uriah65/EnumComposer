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
            string message = string.Format("{0} EnumComposer: ", DateTime.Now.ToString("HH:mm:ss"));
            message += string.Format(format, arguments);
            System.Diagnostics.Debug.WriteLine(message);
        }


        public static string ExceptionMessage(Exception ex)
        {
            string message = "";
            while (ex != null)
            {
                message += Environment.NewLine;
                message += "Exception has occurred." + Environment.NewLine;
                message += "Message:  " + ex.Message + Environment.NewLine;
                message += "Stack:    " + ex.StackTrace + Environment.NewLine;
                ex = ex.InnerException;
            }
            return message;
        }
    }
}
