namespace Zork.Common
{
    public interface IOutputService
    {
        void WriteLine(string message);

        void Write(string message);

        void WriteLine(object obj);

        void Write(object obj);
    }
}
