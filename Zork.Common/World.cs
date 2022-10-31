using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Zork.Common
{
    public class World
    {
        public Room[] Rooms { get; set; }

        public Item[] Items { get; }

        [JsonIgnore]
        public Dictionary<string, Room> RoomsByName { get; }

        [JsonIgnore]
        public Dictionary<string, Item> ItemsByName { get; }

        public World(Room[] rooms, Item[] items)
        {
            Rooms = rooms;
            Items = items;

            RoomsByName = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            foreach (Room room in Rooms)
            {
                RoomsByName.Add(room.Name, room);
            }

            ItemsByName = new Dictionary<string, Item>(StringComparer.OrdinalIgnoreCase);
            foreach(Item item in Items)
            {
                ItemsByName.Add(item.Name, item);
            }
        }

        public Player SpawnPlayer() => new Player(this, StartingLocation);

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateInventory(this);
            }
        }

        [JsonProperty]
        private string StartingLocation { get; set; }

    }
}