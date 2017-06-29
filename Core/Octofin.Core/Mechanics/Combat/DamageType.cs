using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Mechanics.Combat
{
    /// <summary>
    /// Determines damage type of attack or skill.  Pierce, Slash and Crush are counted as Physical Damage.
    /// The rest are counted as elemental damage.
    /// </summary>
    public enum DamageType
    {
        // Physical
        Pierce,
        Slash,
        Crush,

        // Elemental
        Fire,
        Ice,
        Nature,
        Electricity,
        Light,
        Dark
    }
}
