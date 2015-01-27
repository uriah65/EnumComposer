using System;

namespace EnumComposer
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlSelectAttribute : Attribute
    {
        public EnumSqlSelectAttribute(string selectSQL)
        {
        }
    }
}