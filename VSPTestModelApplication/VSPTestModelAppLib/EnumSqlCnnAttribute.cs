using System;

namespace VSPTestModelAppLib
{
    [AttributeUsage(AttributeTargets.Enum)]
    public class EnumSqlCnnAttribute : Attribute
    {
        public EnumSqlCnnAttribute(string sqlServer, string sqlDatabase)
        {
        }
    }
}