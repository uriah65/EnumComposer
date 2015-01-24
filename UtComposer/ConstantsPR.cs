using System;

namespace UtComposer
{
    public static class ConstantsPR
    {
        public static bool IsDell
        {
            get { return Environment.MachineName.ToLower() == "dellstudioxps"; }
        }

        public static string RemoveTextBetween(this string text, string fromPart, string toPart)
        {
            int ix1 = text.IndexOf(fromPart) + fromPart.Length ;
            int ix2 = text.IndexOf(toPart, ix1);// + toPart.Length;

            string result = text.Remove(ix1, ix2 - ix1);
            return result;
        }
    }
}