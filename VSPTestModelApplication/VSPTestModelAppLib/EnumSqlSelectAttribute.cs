using System;

namespace VSPTestModelAppLib
{
    [System.AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlSelectAttribute : Attribute
    {
        public EnumSqlSelectAttribute(string selectSQL)
        {
        }
    }
}