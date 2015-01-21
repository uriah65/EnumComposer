using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer
{
    [System.AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlSelectAttribute : Attribute
    {
        private string _selectSQL;

        public EnumSqlSelectAttribute(string selectSQL)
        {
            _selectSQL = selectSQL;
        }
    }
}
