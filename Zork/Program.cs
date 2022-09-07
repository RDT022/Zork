using System;
using System.Runtime.CompilerServices;

namespace Zork
{
    class Program
    {
        private static string[] Rooms = new string[] { "Forest", "West of House", "Behind House", "Clearing", "Canyon View" };
        private static byte RoomIndex = 1;
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");
            Commands command = Commands.UNKNOWN;
            while (command != Commands.QUIT)
            {
                Console.WriteLine(Rooms[RoomIndex]);
                Console.Write("> ");
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
                        outputString = "The way is shut!";
                        break;
                    case Commands.WEST:
                    case Commands.EAST:
                        if(Move(command))
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
            if(command == Commands.WEST && RoomIndex - 1 >= 0)
            {
                RoomIndex--;
                return true;
            }
            else if(command == Commands.EAST && RoomIndex + 1 < Rooms.Length)
            {
                RoomIndex++;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
