using EnumComposer;

namespace IEnumComposer
{
    public enum DbTypeEnum
    {
        SqlServer,
        Oledb,
        Odbc,
        BuildInFake
    }

    public interface IEnumDbReader
    {
        void ReadEnumeration(EnumModel model);
    }
}