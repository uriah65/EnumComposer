using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    class Fake32_
    {
        [EnumSqlSelect("SELECT * FROM [Fake32_OdbcData.txt]")]
        public enum E
        {
        }
    }
}
