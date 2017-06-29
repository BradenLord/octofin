using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Actors
{
    [Serializable]
    public class Player : Actor
    {
        public Player(string name)
            : base(name)
        {

        }
    }
}
