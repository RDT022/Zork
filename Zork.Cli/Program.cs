using Newtonsoft.Json;
using System.IO;
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

            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(gameFilename));
            game.Run(input, output);

            while (game.IsRunning)
            {
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

