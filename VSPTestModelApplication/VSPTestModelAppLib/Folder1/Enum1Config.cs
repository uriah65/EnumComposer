namespace VSPTestModelAppLib.Folder1
{
    internal class Enum1Config
    {
        [EnumSqlSelect("SELECT ContactTypeID, Name FROM Person.ContactType")]
        public enum ContactTypeEnum
        {
        }

        [EnumSqlCnn("#CONFIG", @"Access")]
        [EnumSqlSelect("SELECT id, name, notes FROM T_Colors")]
        public enum AccessEnum
        {
        }

        [EnumSqlCnn("#CONFIG", @"Text")]
        [EnumSqlSelect(@"SELECT * FROM [data.csv]")]
        public enum OdbcEnum
        {
        }
    }
}