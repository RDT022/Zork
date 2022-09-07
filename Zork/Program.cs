using System;
using System.Runtime.CompilerServices;

namespace Zork
{
    class Program
    {
        private static readonly string[,] _rooms = {
            { "Rocky Trail", "South of House", "Canyon View"},
            { "Forest", "West of House", "Behind House"},
            { "Dense Woods", "North of House", "Clearing"}
        };
        private static byte _roomIndex = 1;
        private static byte _vertIndex = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.Write($"{_rooms[_roomIndex,_vertIndex]}\n> ");
                command = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (command)
                {
                    case Commands.LOOK:
                        outputString = "This is an open field west of a white house, with a boarded front door. \nA rubber mat saying 'Welcome to Zork!' lies by the door.";
                        break;
                    case Commands.QUIT:
                        outputString = "Thank you for playing!";
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.WEST:
                    case Commands.EAST:
                        if (Move(command))
                        {
                            outputString = $"You moved {command}.";
                        }
                        else
                        {
                            outputString = "The way is shut!";
                        }
                        break;
                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString)
        {
            return Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;
        }

        private static bool Move(Commands command)
        {
            bool didMove;
            switch (command)
            {
                case Commands.NORTH when _vertIndex < _rooms.GetLength(0) -1:
                    _vertIndex++;
                    didMove = true;
                    break;
                case Commands.SOUTH when _vertIndex > 0:
                    _vertIndex--;
                    didMove = true;
                    break;
                case Commands.EAST when _roomIndex < _rooms.GetLength(1) - 1:
                    _roomIndex++;
                    didMove = true;
                    break;
                case Commands.WEST when _roomIndex > 0:
                    _roomIndex--;
                    didMove = true;
                    break;
                default:
                    didMove = false;
                    break;
            }
            return didMove;
        }
    }
}
