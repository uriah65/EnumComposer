﻿using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    class Fake_CsFakeDb
    {
        [EnumSqlCnn("FakeSqlServer", "FakeDb")]
        [EnumSqlSelect("SELECT id, name FROM T_Weekdays")]
        public enum Weekdays
        {
        }
    }
}
