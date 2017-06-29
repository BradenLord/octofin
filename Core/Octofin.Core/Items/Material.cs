using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items
{
    /// <summary>
    /// Gives clue as to what materials are required to craft/repair and what materials
    /// get salvaged and provides an additional sorting metric to a given item.
    /// </summary>
    public enum Material
    {
        Unique,

        //Non-Combat Metals
        Brass,
        Silver,
        Gold,

        //Combat Metals
        Iron,
        Steel,
        Mithril,

        //Leathers
        LightLeather,
        HeavyLeather,
        Avianskin,

        //Cloths
        Cloth,
        Padded,
        MithrilPadded,

        //Consumables
        Natural,
        Magical,
    }
}
