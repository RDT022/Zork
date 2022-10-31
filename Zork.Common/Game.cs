using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; private set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        public IOutputService Output { get; private set; }

        public Game(World world, Player player)
        {
           World = world;
           Player = player;
        }

        public void Run(IOutputService output)
        {
            Output = output;

            Room previousRoom = null;
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Output.WriteLine(Player.Location);
                if (previousRoom != Player.Location)
                {
                    Output.WriteLine(Player.Location.Description);
                    foreach(Item item in Player.Location.Inventory)
                    {
                        Output.WriteLine(item.LookDescription);
                    }
                    previousRoom = Player.Location;
                }
                Output.Write("> ");
                string inputString = Console.ReadLine().Trim();
                const char separator = ' ';
                string[] commandTokens = inputString.Split(separator);
                string subject = null;
                switch (commandTokens.Length)
                {
                    case 0:
                        continue;
                    case 1:
                        command = ToCommand(commandTokens[0]);
                        break;
                    case 2:
                        command = ToCommand(commandTokens[0]);
                        subject = commandTokens[1];
                        break;
                    default:
                        break;
                }
                Item thing = null;
                if (subject != null && World.ItemsByName.TryGetValue(subject, out Item i))
                {
                    thing = i;
                }
                string outputString;
                StringBuilder sb = new StringBuilder();
                switch (command)
                {
                    case Commands.LOOK:
                        sb.Append($"{Player.Location.Description}\n");
                        foreach (Item item in Player.Location.Inventory)
                        {
                            sb.Append($"{item.LookDescription}\n");
                        }
                        outputString = sb.ToString();
                        break;
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.WEST:
                    case Commands.EAST:
                        Directions direction = Enum.Parse<Directions>(command.ToString(), true);
                        if (Player.Move(direction))
                        {
                            outputString = $"You moved {command}.\n";
                        }
                        else
                        {
                            outputString = "The way is shut!\n";
                        }
                        break;
                    case Commands.SCORE:
                        outputString = $"Your score would be {Player.Score} in {Player.Moves} move(s).\n";
                        break;
                    case Commands.REWARD:
                        outputString = "Score increased.\n";
                        Player.Score++;
                        break;
                    case Commands.INVENTORY:
                        if(Player.Inventory.Count == 0)
                        {
                            outputString = "You are empty handed.\n";
                        }
                        else
                        {
                            foreach(Item item in Player.Inventory)
                            {
                                sb.Append($"{item.InvDescription}\n");
                            }
                            outputString = sb.ToString();
                        }
                        break;
                    case Commands.TAKE:
                        if (subject == null)
                        {
                            outputString = "Please specify an item to take.\n";
                        }
                        else
                        {
                            outputString = Player.AddToInventory(thing);
                        }
                        break;
                    case Commands.DROP:
                        if (subject == null)
                        {
                            outputString = "Please specify an item to drop.\n";
                        }
                        else
                        {
                            outputString = Player.RemoveFromInventory(thing);
                        }
                        break;
                    default:
                        outputString = "Unknown command.\n";
                        break;
                }

                if(command != Commands.UNKNOWN)
                {
                    Player.Moves++;
                }

                Output.WriteLine(outputString);
            }
        }

        public static Game Load(string filename)
        {
            Game game = JsonConvert.DeserializeObject<Game>(File.ReadAllText(filename));
            game.Player = game.World.SpawnPlayer();
            return game;
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        }
    }  
}
