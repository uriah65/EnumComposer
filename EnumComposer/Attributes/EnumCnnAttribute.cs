using System;

namespace EnumComposer
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlCnnAttribute : Attribute
    {
        public EnumSqlCnnAttribute(string sqlServer, string sqlDatabase)
        {
        }
    }
}