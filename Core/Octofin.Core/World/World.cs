using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Octofin.Core.Utility.Cache;

namespace Octofin.Core.World
{
    [Serializable]
    public class World : BinaryData
    {
        public World(string name)
            : base(name)
        {

        }
    }
}
