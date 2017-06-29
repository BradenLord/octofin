using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Octofin.Core.Mechanics.Combat;

namespace Octofin.Core.Items
{
    [Serializable]
    public class Weapon : Equipment 
    {
        public Attack primaryAttack;
        public Attack secondaryAttack;

        public bool twoHanded;

        public Weapon(String name)
            : base(name, Slot.Weapon)
        { }
    }
}
