using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octofin.Core.Items;
using Octofin.Core.Utility.Cache;

namespace Octofin.Core.Actors
{
    [Serializable]
    public abstract class Actor : BinaryData
    {
        EquipmentSet equipment = new EquipmentSet();

        public Actor(string name)
            : base(name)
        {

        }
    }
}
