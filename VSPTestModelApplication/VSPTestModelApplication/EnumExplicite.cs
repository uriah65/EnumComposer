﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VSPTestModelAppLib;

namespace VSPTestModelApplication
{
    class EnumExplicite
    {

        [EnumSqlCnn("#OleDb", @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=c:\temp\database.accdb;Persist Security Info=False")]
        [EnumSqlSelect("SELECT id, name, notes FROM T_Colors")]
        public enum AccessEnum
        {
        }

        [EnumSqlCnn("#ODBC", @"Driver={Microsoft Text Driver (*.txt; *.csv)};Dbq=c:\temp\;Extensions=asc,csv,tab,txt;Persist Security Info=False")]
        [EnumSqlSelect(@"SELECT * FROM [data.csv]")]
        public enum OdbcEnum
        {
        }

        [EnumSqlCnn("(local)", "AdventureWorks2014")]
        [EnumSqlSelect("SELECT ContactTypeID, Name, Name FROM Person.ContactType")]
        public enum ContactType2Enum
        {
        }
    }
}
