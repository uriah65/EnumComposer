using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Method)]
    public class EnumReverseSqlSelectAttribute : Attribute
    {
        public EnumReverseSqlSelectAttribute(string selectSQL) { }
    }
}
