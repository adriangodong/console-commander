using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConsoleCommander
{
    internal class CompletableReadLine
    {
        private readonly TreeNode<string> completionTreeRootNode;
        private readonly StringBuilder builder;

        private string lastUserInput;
        private IEnumerable<TreeNode<string>> activeCompletions;
        private int completionIndex;

        public CompletableReadLine(TreeNode<string> completionTreeRootNode)
        {
            this.completionTreeRootNode = completionTreeRootNode;
            builder = new StringBuilder();
            lastUserInput = string.Empty;
            activeCompletions = completionTreeRootNode.Children;
        }

        /// <returns>True if loop should continue. If False, get final user input by calling GetReadLine().</returns>
        public bool ReadKey(ConsoleKeyInfo input)
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

        public string GetReadLine()
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

        private IEnumerable<TreeNode<string>> GetValidCompletionTree()
        {
            var currentNode = completionTreeRootNode;
            var lastBlock = ParseLastUserInputLastBlock();

            // Trim last block from completion tree calculation since it is an unfinished block
            var commitedUserInput = lastBlock.prefix.Length > 0 ?
                lastUserInput.Remove(lastBlock.index) :
                lastUserInput;

            foreach (var block in commitedUserInput.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries))
            {
                TreeNode<string> matchedNode = null;

                foreach (var node in currentNode.Children)
                {
                    if (node.Value == block)
                    {
                        matchedNode = node;
                    }
                }

                if (matchedNode == null)
                {
                    return new List<TreeNode<string>>();
                }

                currentNode = matchedNode;
            }

            return currentNode.Children;
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
    }
}
