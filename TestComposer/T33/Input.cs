using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    class Fake33_
    {
        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source =..\..\T33\AccessTest.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name FROM T_Colors")]
        public enum ColorsEnum
        {
        }

        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source =..\..\T33\AccessTest.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name, description FROM T_Colors")]
        public enum ColorsWithDescriptionEnum
        {
            //Red = 1,
            Blue = 2,
            Green = 3,
        }
    }
}
