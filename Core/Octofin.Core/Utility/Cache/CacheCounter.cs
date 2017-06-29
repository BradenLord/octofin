using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Utility.Cache
{
    /// <summary>
    /// Used to update guis with operation performance.
    /// </summary>
    public class CacheCounter
    {
        public bool operationsFinished = false;
        public string currentOperation;
        public int totalObjects = 1;
        public int consumedObjects = 0;
    }
}
