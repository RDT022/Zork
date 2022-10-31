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
        public IReadOnlyDictionary<string, Room> RoomsByName => mRoomsByName;

        [JsonIgnore]
        public Dictionary<string, Item> ItemsByName { get; }

        public World(Room[] rooms, Item[] items)
        {
            Rooms = rooms;
            Items = items;

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
            mRoomsByName = Rooms.ToDictionary(room => room.Name, room => room);

            foreach (Room room in Rooms)
            {
                room.UpdateNeighbors(this);
                room.UpdateInventory(this);
            }
        }

        [JsonProperty]
        private string StartingLocation { get; set; }

        private Dictionary<string, Room> mRoomsByName;
    }
}