using System;

namespace EnumComposer
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Enum)]
    public class EnumSqlCnnAttribute : Attribute
    {
        public EnumSqlCnnAttribute(string sqlServer, string sqlDatabase)
        {
        }
    }
}