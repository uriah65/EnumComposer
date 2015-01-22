using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer
{
    public interface IEnumLog
    {
        void WriteLine(string format, params object[] arguments);
    }
}
