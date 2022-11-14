using Newtonsoft.Json;
using System.IO;
using System;
using System.Text;
using System.Linq;

namespace Zork.Common
{
    public class Game
    {
        public World World { get; private set; }

        [JsonIgnore]
        public Player Player { get; private set; }

        [JsonIgnore]
        public IOutputService Output { get; private set; }

        [JsonIgnore]
        public IInputService Input { get; private set; }

        [JsonIgnore]
        public bool IsRunning { get; private set; }

        public Room PreviousRoom { get; private set; }

        public Game(World world, string startingLocation)
        {
            World = world;
            Player = new Player(World, startingLocation);
        }

        public void Run(IInputService input, IOutputService output)
        {
            Output = output;
            Input = input;

            IsRunning = true;
            Input.InputReceived += Input_InputReceived;
            Output.WriteLine("Welcome to Zork!");
            Output.WriteLine($"{Player.Location}");
            Look();
        }

        private void Input_InputReceived(object sender, string inputString)
        {
            Commands command = Commands.UNKNOWN;
            PreviousRoom = Player.Location;
            const char separator = ' ';
            string[] commandTokens = inputString.Split(separator);
            string subject = null;
            switch (commandTokens.Length)
            {
                case 0:
                    break;
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
            switch (command)
            {
                case Commands.LOOK:
                    Look();
                    break;
                case Commands.QUIT:
                    Output.WriteLine("Thank you for playing!");
                    IsRunning = false;
                    break;
                case Commands.NORTH:
                case Commands.SOUTH:
                case Commands.WEST:
                case Commands.EAST:
                    Directions direction = (Directions)command;
                    Output.WriteLine(Player.Move(direction) ? $"You moved {direction}.\n" : "The way is shut!\n");
                    break;
                case Commands.SCORE:
                    Output.WriteLine($"Your score would be {Player.Score} in {Player.Moves} move(s).\n");
                    break;
                case Commands.REWARD:
                    Output.WriteLine("Score increased.\n");
                    Player.Score++;
                    break;
                case Commands.INVENTORY:
                    if (Player.Inventory.Count == 0)
                    {
                        Output.WriteLine("You are empty handed.\n");
                    }
                    else
                    {
                        Output.WriteLine("You are carrying:");
                        foreach (Item item in Player.Inventory)
                        {
                            Output.WriteLine($"{item.InvDescription}\n");
                        }
                    }
                    break;
                case Commands.TAKE:
                    if (subject == null)
                    {
                        Output.WriteLine("Please specify an item to take.\n");
                    }
                    else
                    {
                        Output.WriteLine(Player.AddToInventory(thing));
                    }
                    break;
                case Commands.DROP:
                    if (subject == null)
                    {
                        Output.WriteLine("Please specify an item to drop.\n");
                    }
                    else
                    {
                        Output.WriteLine(Player.RemoveFromInventory(thing));
                    }
                    break;
                default:
                    Output.WriteLine("Unknown command.\n");
                    break;
            }

            if (command != Commands.UNKNOWN)
            {
                Player.Moves++;
            }

            Output.WriteLine($"{Player.Location}");
            if (ReferenceEquals(PreviousRoom, Player.Location) == false)
            {
                Look();
            }

        }


        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        }

        private void Look()
        {
            Output.WriteLine(Player.Location.Description);
            foreach(Item item in Player.Location.Inventory)
            {
                Output.WriteLine(item.LookDescription);
            }
        }
    }
}
