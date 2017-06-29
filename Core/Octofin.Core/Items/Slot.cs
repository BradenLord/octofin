using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items
{
    /// <summary>
    /// Determines what kind of equipment slot and item can be applied to.  Also used in tags for sorting.
    /// </summary>
    public enum Slot
    {
        Weapon,
        Secondary,
        Headgear,
        UpperBody,
        LowerBody,
        Footgear,
        Armgear,
        Enhancement,
        Consumable,
    }
}
