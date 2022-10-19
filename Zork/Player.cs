﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork
{
    public class Player
    {
        public World World { get; }

        [JsonIgnore]
        public Room Location { get; private set; }

        public int Score { get; set; }

        public int Moves { get; set; }

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
    }
}
