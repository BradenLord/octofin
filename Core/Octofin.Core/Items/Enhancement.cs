using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items
{
    [Serializable]
    public class Enhancement : Equipment
    {
        public Enhancement(string name)
            : base(name, Slot.Enhancement)
        {

        }
    }
}
