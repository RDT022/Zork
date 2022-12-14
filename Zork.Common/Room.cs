using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Zork.Common
{
    public class Room: IEquatable<Room>
    {
        [JsonProperty(Order = 1)]
        public string Name { get; private set; }

        [JsonProperty(Order = 2)]
        public string Description { get; private set; }

        [JsonProperty(PropertyName = "Neighbors", Order = 3)]
        private Dictionary<Directions, string> NeighborNames { get; set; }

        [JsonIgnore]
        public Dictionary<Directions, Room> Neighbors { get; private set; }

        public List<Item> Inventory { get; private set; }

        private string[] InventoryNames { get; set; }

        public Room(string name, string description, Dictionary<Directions, string> neighborNames, string[] inventoryNames)
        {
            Name = name;
            Description = description;
            NeighborNames = neighborNames ?? new Dictionary<Directions, string>();
            InventoryNames = inventoryNames ?? new string[0];
        }

        public static bool operator ==(Room lhs, Room rhs)
        {
            if(ReferenceEquals(lhs,rhs))
            {
                return true;
            }

            if(lhs is null || rhs is null)
            {
                return false;
            }

            return lhs.Name == rhs.Name;
        }

        public static bool operator !=(Room lhs, Room rhs) => !(lhs == rhs);

        public override bool Equals(object obj) => obj is Room room ? this == room : false;

        public bool Equals(Room other) => this == other;

        public override string ToString() => Name;

        public override int GetHashCode() => Name.GetHashCode();

        public void UpdateNeighbors(World world)
        {
            Neighbors = new Dictionary<Directions, Room>();
            foreach (var neighborName in NeighborNames)
            {
                if (world.RoomsByName.TryGetValue(neighborName.Value, out Room room))
                {
                    Neighbors.Add(neighborName.Key, room);
                }
            }
            NeighborNames = null;
        }

        public void UpdateInventory(World world)
        {
            Inventory = new List<Item>();
            foreach(var inventoryName in InventoryNames)
            {
                Inventory.Add(world.ItemsByName[inventoryName]);
            }
            InventoryNames = null;
        }

    }
}