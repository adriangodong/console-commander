using System.Threading.Tasks;

namespace ConsoleCommander
{
    public interface ICommand
    {
        Task Execute(IOutputService output, ExecutionContext context);
    }
}
