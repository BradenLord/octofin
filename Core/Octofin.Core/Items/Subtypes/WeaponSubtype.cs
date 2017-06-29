using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items.Subtypes
{
    [Serializable]
    public class WeaponSubtype
    {
        static WeaponSubtype()
        {
            Sword = new Subtype("Sword");
            Axe = new Subtype("Axe");
            Bludgeon = new Subtype("Bludgeon");
            Spear = new Subtype("Spear");
            Throwing = new Subtype("Throwing");
            Bow = new Subtype("Bow");
            Magical = new Subtype("Magical");
        }

        public static readonly Subtype Sword;
        public static readonly Subtype Axe;
        public static readonly Subtype Bludgeon;
        public static readonly Subtype Spear;
        public static readonly Subtype Throwing;
        public static readonly Subtype Bow;
        public static readonly Subtype Magical;
    }
}
