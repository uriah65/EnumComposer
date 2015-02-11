using EnumComposer;
using System.ComponentModel;

namespace UtComposer
{
    class Fake32_
    {
        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=..\..\T32;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect("SELECT * FROM [Data.txt]")]
        public enum E
        {
        }

        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=..\..\T32;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect("SELECT * FROM [Data2.csv]")]
        public enum E2
        {
            [Description("#FAEBD7	 	Shades	Mix")]
            AntiqueWhite = 2,
            [Description("#00FFFF	 	Shades	Mix")]
            Aqua = 3,
            Aquamarine = 4,
            Azure = 5,
            [Description("#F5F5DC	 	Shades	Mix")]
            Bisque = 7,
            [Description("#000000	 	Shades	Mix")]
            Black = 8,
        }
    }
}
