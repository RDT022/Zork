using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Player
    {
        public event EventHandler<int> MovesChanged;

        public event EventHandler<Room> LocationChanged;

        [JsonIgnore]
        public Room Location 
        { 
            get => _location; 
            set
            {
                if(_location != value)
                {
                    _location = value;
                    LocationChanged?.Invoke(this, Location);
                }
            }
        }

        public int Score { get; set; }

        public List<Item> Inventory { get; }

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

        public Player(World world, string startingLocation)
        {
            _world = world;
            if (_world.RoomsByName.TryGetValue(startingLocation, out _location) == false)
            {
                throw new Exception($"Invalid starting location: {startingLocation}");
            }
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
                return $"You took the {item}\n";
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
                return $"You dropped the {item}\n";
            }
        }

        private int _moves;

        private readonly World _world;
        private Room _location;
    }
}
