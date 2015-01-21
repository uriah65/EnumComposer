using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    public class ConstantsPR
    {
        public static bool IsDell
        {
            get { return Environment.MachineName.ToLower() == "dellstudioxps"; }
        }

    }
}
