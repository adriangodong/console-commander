using System;

namespace ConsoleCommander
{
    public class ConsoleOutputService : IOutputService
    {
        public void Info(string message)
        {
            Console.WriteLine(message);
        }

        public void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {message}");
            Console.WriteLine();
            Console.ResetColor();
        }

        public void ErrorWithContent(string message, string content)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine($"ERROR: {message}");
            Console.WriteLine(content);
            Console.WriteLine();
            Console.ResetColor();
        }

        public void ExitCommand()
        {
            Console.WriteLine();
        }
    }
}
