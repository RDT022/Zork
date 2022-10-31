using System;
using Zork.Common;

namespace Zork.Cli
{
    class Program
    {
        static void Main(string[] args)
        {
            var output = new ConsoleOutputService();
            var input = new ConsoleInputService();

            const string defaultGameFilename = @"Content\Rooms.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);

            Game game = Game.Load(gameFilename);
            Console.WriteLine("Welcome to Zork!");
            game.Run(input, output);

            while(game.IsRunning)
            {
                game.Output.Write("> ");
                input.ProcessInput();

                if(game.IsRunning)
                {
                    game.Output.WriteLine(game.Player.Location);
                }
            }
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}
