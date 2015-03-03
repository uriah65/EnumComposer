using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSPTestModelAppLib;

namespace VSPTestModelApplication
{
    class EnumMinimalDemo
    {
        [EnumSqlCnn("FakeSqlServer", "FakeDb")]
        [EnumSqlSelect("SELECT id, name, description FROM T_Weekdays")]
        public enum Weekdays
        {
        }

        [EnumSqlCnn("FakeSqlServer", "FakeDb")]
        [EnumSqlSelect("SELECT id, name FROM T_Weekdays")]
        public enum Weekdays2
        {
        }
    }
}
