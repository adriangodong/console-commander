using System.Collections.Generic;

namespace ConsoleCommander
{
    public interface ICommandParser
    {
        List<TreeNode<string>> GetCompletionTree();
        (ICommand command, string error) Parse(string[] commandTokens);
    }
}
