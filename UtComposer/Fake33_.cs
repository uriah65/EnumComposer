﻿using EnumComposer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtComposer
{
    class Fake33_
    {
        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source =..\..\AccessTest.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name FROM T_Colors")]
        public enum MeetingTypeEnum
        {
        }
    }
}
