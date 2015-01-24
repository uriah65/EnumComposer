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
        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=..\..\;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect("SELECT * FROM [Fake32_OdbcData.txt]")]
        public enum E
        {
        }
    }
}
