using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octofin.Core.Utility.Cache;

namespace Octofin.Core.Mechanics.Combat
{
    [Serializable]
    public class Attack
    {
        public Damage[] damages;

        public Attack()
        {
            this.damages = new Damage[] { new Damage() };
        }
    }
}
