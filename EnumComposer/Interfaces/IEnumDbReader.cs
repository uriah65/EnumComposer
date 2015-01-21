using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IEnumComposer
{
    public interface IEnumDbReader
    {
        void ReadEnumeration(EnumModel model);
    }
}
