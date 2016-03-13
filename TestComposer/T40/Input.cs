using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    [EnumSqlCnn("#OleDb", "onClass")]
    class Fake40_
    {
        [EnumSqlCnn("#OleDb", @"onEnum")]
        [EnumSqlSelect("SELECT id, name FROM T_Colors")]
        public enum ColorsEnum
        {
        }

        [EnumSqlCnn("#OleDb", "onMethod")]
        void Method()
        {

        }
        
    }

    [EnumSqlCnn("#OleDb", "onClassB")]
    class Fake40b_
    {
        [EnumSqlCnn("#OleDb", "onMethodB")]
        public string MethodB(int ix)
        {
            return "";
        }

        [EnumSqlCnn("#OleDb", "onMethodC")]
        public string MethodC(int ix)
        {
            return "";
        }

        [EnumSqlCnn("#OleDb", @"onEnumB")]
        public enum ColorsEnum
        {
        }
    }
}
