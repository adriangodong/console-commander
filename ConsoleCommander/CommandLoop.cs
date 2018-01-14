using System;
using System.Threading.Tasks;

namespace ConsoleCommander
{
    public class CommandLoop
    {
        private const string Prompt = "> ";

        private readonly ExecutionContext executionContext = new ExecutionContext();
        private readonly IOutputService outputService;
        private readonly CommandNode rootCommandNode;

        public CommandLoop(IOutputService outputService, CommandNode rootCommandNode)
        {
            this.outputService = outputService;
            this.rootCommandNode = rootCommandNode;
        }

        public void AddCommand(ICommand command)
        {
            executionContext.CommandQueue.Enqueue(command);
        }

        public async Task StartLoop()
        {
            try
            {
                Console.Clear();
            }
            catch (System.IO.IOException)
            {
                // Ignore exception when output is redirected.
                // This is a known issue and workaround.
            }

            while (true)
            {
                ICommand commandToExecute = null;

                // If there are no command in the queue, ask for a new one from user
                if (executionContext.CommandQueue.Count == 0)
                {
                    var completableReadLine = new CompletableReadLine(rootCommandNode);
                    var command = completableReadLine.ReadLine(Prompt);
                    var commandTokens = command.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    var closestCommandNode = rootCommandNode.FindNode(commandTokens);

                    var parseOutput = closestCommandNode.ParseFunction(commandTokens);

                    if (parseOutput.command == null)
                    {
                        outputService.Error(parseOutput.error);
                    }
                    else
                    {
                        commandToExecute = parseOutput.command;
                    }
                }
                else
                {
                    commandToExecute = executionContext.CommandQueue.Dequeue();
                }

                if (commandToExecute != null)
                {
                    await commandToExecute.Execute(outputService, executionContext);
                }
            }
        }
    }
}
