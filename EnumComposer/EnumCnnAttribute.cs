using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer
{

    [System.AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlCnnAttribute : Attribute
    {
        //private string _selectSQL;

        public EnumSqlCnnAttribute(string sqlServer, string sqlDatabase)
        {
            //_selectSQL = selectSQL;
        }
    }

}
