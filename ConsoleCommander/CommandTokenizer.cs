using System;
using System.Collections.Generic;

namespace ConsoleCommander
{
    internal class CommandTokenizer
    {

        public const string Error_EmptyCommand = "Empty command";
        public const string Error_UnknownFirstToken = "Unknown first token '{0}'";

        private readonly List<ICommandParser> commandParsers;

        public CommandTokenizer(List<ICommandParser> commandParsers)
        {
            this.commandParsers = commandParsers;
        }

        public (ICommand command, string error) Parse(string command)
        {
            if (string.IsNullOrWhiteSpace(command))
            {
                return (null, Error_EmptyCommand);
            }

            var tokens = command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var commandParser in commandParsers)
            {
                var parseOutput = commandParser.Parse(tokens);
                if (parseOutput.command != null)
                {
                    return (parseOutput.command, null);
                }

                if (parseOutput.error != null)
                {
                    return (null, parseOutput.error);
                }
            }

            return (null, string.Format(Error_UnknownFirstToken, tokens[0]));
        }

    }
}
