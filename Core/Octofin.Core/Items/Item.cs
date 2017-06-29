using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Octofin.Core.Mechanics;
using Octofin.Core.Assets;
using Octofin.Core.Items.Subtypes;
using Octofin.Core.Utility.Cache;

namespace Octofin.Core.Items
{
    [Serializable]
    public abstract class Item : BinaryData
    {
        public string label;
        public Stats stats;

        public Icon icon;

        //Tag management
        public Material material;
        public readonly Slot slot;
        public Subtype subtype;

        public Item(String name, Slot slot)
            : base(name)
        {
            this.slot = slot;
        }
    }
}
