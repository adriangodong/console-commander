using System;
using System.Collections.Generic;

namespace ConsoleCommander
{
    internal class CommandTokenizer
    {

        public const string Error_EmptyCommand = "Empty command";
        public const string Error_UnknownFirstToken = "Unknown first token '{0}'";

        private readonly List<ICommandParser> commandParsers;
        private readonly string prompt;

        public CommandTokenizer(List<ICommandParser> commandParsers, string prompt)
        {
            this.commandParsers = commandParsers;
            this.prompt = prompt;
        }

        public string ReadWithTabCompletion()
        {
            var completableReadLine = new CompletableReadLine(GenerateCompletionTree(commandParsers));

            // Reset console line
            ClearCurrentConsoleLine();
            Console.Write(prompt);

            while (completableReadLine.ReadKey(Console.ReadKey()))
            {
                ClearCurrentConsoleLine();
                Console.Write(prompt + completableReadLine.GetReadLine());
            }

            return completableReadLine.GetReadLine();
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

        private static TreeNode<string> GenerateCompletionTree(IEnumerable<ICommandParser> commandParsers)
        {
            var commandTreeNodes = new List<TreeNode<string>>();
            foreach (var commandParser in commandParsers)
            {
                commandTreeNodes.AddRange(commandParser.GetCompletionTree());
            }
            return new TreeNode<string>(null, commandTreeNodes);
        }

        private static void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

    }
}
