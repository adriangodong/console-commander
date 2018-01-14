using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommander
{
    internal class CompletableReadLine
    {
        private readonly CommandNode rootCommandNode;
        private readonly StringBuilder builder;

        private string lastUserInput;
        private IEnumerable<CommandNode> activeCompletions;
        private int completionIndex;

        public CompletableReadLine(CommandNode rootCommandNode)
        {
            this.rootCommandNode = rootCommandNode;
            builder = new StringBuilder();
            lastUserInput = string.Empty;
            activeCompletions = rootCommandNode.Children;
        }

        public string ReadLine(string prompt)
        {
            var completableReadLine = new CompletableReadLine(rootCommandNode);

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

        /// <returns>True if loop should continue. If False, get final user input by calling GetReadLine().</returns>
        internal bool ReadKey(ConsoleKeyInfo input)
        {
            switch (input.Key)
            {
                case ConsoleKey.Enter:
                    Console.WriteLine();
                    return false;
                case ConsoleKey.Tab:
                    SuggestBlock();
                    break;
                case ConsoleKey.Backspace:
                    TrimChar();
                    break;
                default:
                    AppendChar(input.KeyChar);
                    break;
            }

            return true;
        }

        internal string GetReadLine()
        {
            return builder.ToString();
        }

        private void AppendChar(char input)
        {
            builder.Append(input);
            UpdateLastUserInput();
        }

        private void TrimChar()
        {
            if (builder.Length > 0)
            {
                builder.Remove(builder.Length - 1, 1);
                UpdateLastUserInput();
            }
        }

        private void UpdateLastUserInput()
        {
            lastUserInput = builder.ToString();
            activeCompletions = GetValidCompletionTree();
            completionIndex = 0;
        }

        private void SuggestBlock()
        {
            if (activeCompletions.Any())
            {
                var lastBlock = ParseLastUserInputLastBlock();
                var completionCandidates = activeCompletions.Where(node => node.Value.StartsWith(lastBlock.prefix)).ToList();
                if (completionCandidates.Count == 0)
                {
                    return;
                }

                if (completionCandidates.Count <= completionIndex)
                {
                    completionIndex = 0;
                }

                builder.Remove(lastBlock.index, builder.Length - lastBlock.index).Append(completionCandidates[completionIndex++].Value);
            }
        }

        private IEnumerable<CommandNode> GetValidCompletionTree()
        {
            var lastBlock = ParseLastUserInputLastBlock();

            // Trim last block from completion tree calculation since it is an unfinished block
            var commitedUserInput = lastBlock.prefix.Length > 0 ?
                lastUserInput.Remove(lastBlock.index) :
                lastUserInput;

            var matchedCommandNode = rootCommandNode.FindNode(
                commitedUserInput.Split(
                    new[] { " " },
                    StringSplitOptions.RemoveEmptyEntries),
                exact: true);

            return matchedCommandNode?.Children ?? new List<CommandNode>();
        }

        private (string prefix, int index) ParseLastUserInputLastBlock()
        {
            var lastIndexOfSeparator = lastUserInput.LastIndexOf(" ", StringComparison.OrdinalIgnoreCase);
            if (lastIndexOfSeparator == -1)
            {
                return (lastUserInput, 0);
            }
            else
            {
                var lastIndexOfBlock = lastIndexOfSeparator + 1;
                return (lastUserInput.Substring(lastIndexOfBlock, lastUserInput.Length - lastIndexOfBlock), lastIndexOfBlock);
            }
        }

        private static void ClearCurrentConsoleLine()
        {
            var currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
