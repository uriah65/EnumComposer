using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnumComposer.Attributes
{
    [System.AttributeUsage(AttributeTargets.Field)]
    public class EnumDictionarySqlSelectAttribute : Attribute
    {
        public EnumDictionarySqlSelectAttribute(string selectSQL) { }
    }
}
