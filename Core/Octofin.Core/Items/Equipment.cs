using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items
{
    [Serializable]
    public abstract class Equipment : Item
    {
        public Quality quality;
        public Enhancement[] enhancements;

        public Equipment(String name, Slot slot)
            : base(name, slot)
        {
            quality = Quality.Average;
        }
    }
}
