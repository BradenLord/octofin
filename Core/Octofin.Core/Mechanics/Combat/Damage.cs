using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Mechanics.Combat
{
    [Serializable]
    public class Damage
    {
        public DamageType damageType;
        public int minimumDamage;
        public int maximumDamage;

        public bool damageOverTime;
        public float dotTime;

        public Damage() 
            : this (DamageType.Pierce, 0, 1)
        {}

        public Damage(DamageType damageType, int minimumDamage, int maximumDamage)
           : this (damageType, minimumDamage, maximumDamage, false, 0.0F)
        {}

        public Damage(DamageType damageType, int minimumDamage, int maximumDamage, bool damageOverTime, float dotTime)
        {
            this.damageType = damageType;
            this.minimumDamage = minimumDamage;
            this.maximumDamage = maximumDamage;
            this.damageOverTime = damageOverTime;
            this.dotTime = dotTime;
        }
    }
}
