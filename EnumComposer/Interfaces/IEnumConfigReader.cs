using System;

namespace EnumComposer
{
    public interface IEnumConfigReader
    {
        Tuple<string, string> GetConnectionString(string connectionStringName);
    }
}