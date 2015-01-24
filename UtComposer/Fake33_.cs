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
        [EnumSqlCnn("#OleDb", "Provider=sqloledb;Data Source=DELLSTUDIOXPS;Initial Catalog=Arctics;Integrated Security=SSPI;")]
        [EnumSqlSelect("SELECT mt__id, mt_name FROM T_MeetingTypes")]
        public enum MeetingTypeEnum
        {
        }
    }
}
