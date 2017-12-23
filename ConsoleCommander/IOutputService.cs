namespace ConsoleCommander
{
    public interface IOutputService
    {
        void Info(string message);
        void Success(string message);
        void Error(string message);
        void ErrorWithContent(string message, string content);
        void ExitCommand();
    }
}
