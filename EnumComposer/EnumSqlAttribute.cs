using System;

namespace EnumComposer
{
    [System.AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlSelectAttribute : Attribute
    {
        public EnumSqlSelectAttribute(string selectSQL)
        {
        }
    }
}