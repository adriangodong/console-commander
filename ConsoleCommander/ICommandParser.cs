namespace ConsoleCommander
{
    public interface ICommandParser
    {
        (ICommand command, string error) Parse(string[] commandTokens);
    }
}
