using System;
using System.Collections.Generic;
using System.Text;

namespace Zork.Common
{
    public class Item
    {
        public string Name { get; }

        public string InvDescription { get; }

        public string LookDescription { get; }

        public Item(string name, string invdescription, string lookdescription)
        {
            Name = name;
            InvDescription = invdescription;
            LookDescription = lookdescription;
        }

        public override string ToString() => Name;
    }
}
