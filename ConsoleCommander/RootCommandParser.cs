namespace ConsoleCommander
{
    public class RootCommandParser : ICommandParser
    {
        private const string Error_EmptyCommand = "Empty command";
        private const string Error_UnknownFirstToken = "Unknown first token '{0}'";

        public (ICommand command, string error) Parse(string[] commandTokens)
        {
            if (commandTokens.Length == 0)
            {
                return (null, Error_EmptyCommand);
            }

            return (null, Error_UnknownFirstToken);
        }
    }
}
