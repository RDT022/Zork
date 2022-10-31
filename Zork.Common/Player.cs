using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<int> MovesChanged;

        public World World { get; }

        [JsonIgnore]
        public Room Location { get; private set; }

        public int Score { get; set; }

        public int Moves
        {
            get
            {
                return _moves;
            }
            set
            {
                if(_moves != value)
                {
                    _moves = value;
                    MovesChanged?.Invoke(this, _moves);
                }
            }
        }

        [JsonIgnore]
        public string LocationName
        {
            get
            {
                return Location?.Name;
            }
            set
            {
                Location = World?.RoomsByName.GetValueOrDefault(value);
            }
        }

        public List<Item> Inventory { get; }

        public Player(World world, string startingLocation)
        {
            World = world;
            LocationName = startingLocation;
            Inventory = new List<Item>();
        }

        public bool Move(Directions direction)
        {
            bool isValidMove = Location.Neighbors.TryGetValue(direction, out Room destination);
            if(isValidMove)
            {
                Location = destination;
            }

            return isValidMove;
        }

        public string AddToInventory(Item item)
        {
            if(Location.Inventory.Count == 0)
            {
                return "There is nothing to take.\n";
            }
            else if(item == null || !Location.Inventory.Contains(item))
            {
                return "You can see no such thing.\n";
            }
            else
            {
                Inventory.Add(item);
                Location.Inventory.Remove(item);
                return $"You took the {item.Name}\n";
            }
        }

        public string RemoveFromInventory(Item item)
        {
            if (Inventory.Count == 0)
            {
                return "You are not carrying anything.\n";
            }
            else if (item == null || !Inventory.Contains(item))
            {
                return "You are carrying no such thing.\n";
            }
            else
            {
                Location.Inventory.Add(item);
                Inventory.Remove(item);
                return $"You dropped the {item.Name}\n";
            }
        }

        private int _moves;
    }
}
