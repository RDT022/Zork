using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            const string defaultGameFilename = @"Content\Rooms.json";
            string gameFilename = (args.Length > 0 ? args[(int)CommandLineArguments.GameFilename] : defaultGameFilename);

            Game game = Game.Load(gameFilename);
            Console.WriteLine("Welcome to Zork!");
            game.Run();
        }

        private enum CommandLineArguments
        {
            GameFilename = 0
        }
    }
}
