using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Octofin.Core.Utility.Cache
{
    [Serializable]
    public abstract class BinaryData
    {
        // Dictonary entry string
        public readonly string name;

        public BinaryData(String name)
        {
            this.name = name;
        }
    }
}
