using System;

namespace EnumComposer
{
    [System.AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlSelectAttribute : Attribute
    {
        //private string _selectSQL;
        public EnumSqlSelectAttribute(string selectSQL)
        {
            //_selectSQL = selectSQL;
        }
    }
}