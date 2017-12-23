using System.Collections.Generic;

namespace ConsoleCommander
{
    public class ExecutionContext
    {
        public Queue<ICommand> CommandQueue { get; }

        public ExecutionContext()
        {
            CommandQueue = new Queue<ICommand>();
        }
    }
}
