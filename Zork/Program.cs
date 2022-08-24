using System;

namespace Zork
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Zork!");

            string InputString = Console.ReadLine().Trim().ToUpper();

            if(InputString == "QUIT")
            {
                Console.WriteLine("Thank you for playing!");
            }
            else if(InputString == "LOOK")
            {
                Console.WriteLine("This is an open field west of a white house, with a boarded front door.\nA rubber mat saying 'Welcome to Zork!' lies by the door.");
            }
            else
            {
                Console.WriteLine($"Unrecognized Command: {InputString}");
            }
        }
    }
}
