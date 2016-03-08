using System;

namespace EnumComposer
{
    public class DedbugLog : IEnumLog
    {
        public void WriteLine(string format, params object[] arguments)
        {
            string message = string.Format("{0} EnumComposer: ", DateTime.Now.ToString("HH:mm:ss"));
            if (arguments.Length != 0)
            {
                message += string.Format(format, arguments);
            }
            else
            {
                message = format;
            }
            System.Diagnostics.Debug.WriteLine(message);
        }

        public static string ExceptionMessage(Exception ex)
        {
            string message = "";
            while (ex != null)
            {
                message += $"Exception has occurred.{Environment.NewLine}";
                message += $"Message:  {ex.Message}{Environment.NewLine}";
                message += $"Stack:    {ex.StackTrace}{Environment.NewLine}";
                ex = ex.InnerException;
            }
            return message;
        }
    }
}