using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items
{
    /// <summary>
    /// 1 - Weapon A
    /// 2 - Secondary A
    /// 3 - Weapon B
    /// 4 - Secondary B
    /// 5 - Headgear
    /// 6 - UpperBody
    /// 7 - LowerBody
    /// 8 - Armgear
    /// 9 - Footgear
    /// 10 - Accessory A
    /// 11 - Accessory B
    /// 12 - Accessory C
    /// 13 - Accessory D
    /// </summary>
    [Serializable]
    public class EquipmentSet
    {
        public Weapon weaponA;
        public Weapon weaponB;
    }
}
