using System.Collections.Generic;

namespace ConsoleCommander
{
    public class TreeNode<T>
    {
        public TreeNode(T value, IEnumerable<TreeNode<T>> children)
        {
            Value = value;
            Children = children;
        }

        public T Value { get; }
        public IEnumerable<TreeNode<T>> Children { get; }
    }
}