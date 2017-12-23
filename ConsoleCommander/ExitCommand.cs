using System.Threading.Tasks;

namespace ConsoleCommander
{
    public class ExitCommand : ICommand
    {
        public Task Execute(IOutputService output, ExecutionContext context)
        {
            output.Success("Bye!");
            System.Environment.Exit(0);
            return Task.CompletedTask;
        }
    }
}
