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
            //AliceBlue = 1,
            //AntiqueWhite = 2,
            //Aqua = 3,
            //Aquamarine = 4,
            //Azure = 5,
            //Beige = 6,
            //Bisque = 7,
            //Black = 8,
            //BlanchedAlmond = 9,
            //Blue = 10,
            //BlueViolet = 11,
        }

        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=..\..\T32;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect("SELECT * FROM [Data2.csv]")]
        public enum E2
        {
            //[Description("#F0F8FF	 	Shades	Mix")]
            //AliceBlue = 1,
            [Description("#FAEBD7	 	Shades	Mix")]
            AntiqueWhite = 2,
            [Description("#00FFFF	 	Shades	Mix")]
            Aqua = 3,
            [Description("#7FFFD4	 	Shades	Mix")]
            Aquamarine = 4,
            [Description("#F0FFFF	 	Shades	Mix")]
            Azure = 5,
            //[Description("#F5F5DC	 	Shades	Mix")]
            //Beige = 6,
            [Description("#FFE4C4	 	Shades	Mix")]
            Bisque = 7,
            [Description("#000000	 	Shades	Mix")]
            Black = 8,
            //[Description("#FFEBCD	 	Shades	Mix")]
            //BlanchedAlmond = 9,
            //[Description("#0000FF	 	Shades	Mix")]
            //Blue = 10,
            //[Description("#8A2BE2")]
            //BlueViolet = 11,
        }
    }
}
