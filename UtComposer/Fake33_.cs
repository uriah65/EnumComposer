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
    
        [EnumSqlSelect("SELECT id, name FROM T_Colors")]
        public enum MeetingTypeEnum
        {
        }
    }
}
