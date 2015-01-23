namespace EnumComposer
{
    public interface IEnumLog
    {
        void WriteLine(string format, params object[] arguments);
    }
}