using System;

namespace ConsoleApplication1
{
    internal class Program
    {
        [EnumSqlCnn("FakeSqlServer", "FakeDb")]
        [EnumSqlSelect("SELECT id, name FROM T_Weekdays")]
        public enum Weekdays
        {
        }

        private static void Main(string[] args)
        {
        }

        [AttributeUsage(AttributeTargets.Enum)]
        public class EnumSqlCnnAttribute : Attribute
        {
            public EnumSqlCnnAttribute(string sqlServer, string sqlDatabase)
            {
            }
        }

        [System.AttributeUsage(AttributeTargets.Enum)]
        public class EnumSqlSelectAttribute : Attribute
        {
            public EnumSqlSelectAttribute(string selectSQL)
            {
            }
        }
    }
}