using System;
using System.Collections.Generic;

namespace ConsoleCommander
{
    public class CommandNode
    {
        public CommandNode(
            string value,
            Func<string[], (ICommand, string)> parseFunction,
            IEnumerable<CommandNode> children)
        {
            Value = value;
            ParseFunction = parseFunction;
            Children = children;
        }

        public CommandNode(
            string value,
            ICommandParser commandParser,
            IEnumerable<CommandNode> children)
        {
            Value = value;
            ParseFunction = commandParser.Parse;
            Children = children;
        }

        public string Value { get; }
        public Func<string[], (ICommand command, string error)> ParseFunction { get; }
        public IEnumerable<CommandNode> Children { get; }

        /// <param name="exact">If true, must return exact match. If false, will return last matched node.</param>
        public CommandNode FindNode(string[] blocks, bool exact = false)
        {
            var currentNode = this;

            foreach (var block in blocks)
            {
                CommandNode matchedNode = null;

                if (currentNode.Children != null)
                {
                    foreach (var node in currentNode.Children)
                    {
                        if (node.Value == block)
                        {
                            matchedNode = node;
                        }
                    }
                }

                if (matchedNode == null)
                {
                    return exact ? null : currentNode;
                }

                currentNode = matchedNode;
            }

            return currentNode;
        }
    }
}
