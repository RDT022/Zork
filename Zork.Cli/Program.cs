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

            while (game.IsRunning)
            {
                if (game.IsRunning)
                {
                    game.Output.WriteLine(game.Player.Location);
                    if (game.PreviousRoom != game.Player.Location)
                    {
                        game.Output.WriteLine(game.Player.Location.Description);
                        foreach (Item item in game.Player.Location.Inventory)
                        {
                            game.Output.WriteLine(item.LookDescription);
                        }
                    }
                }
                game.Output.Write("> ");
                input.ProcessInput();
            }
            

        }
        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }

}

