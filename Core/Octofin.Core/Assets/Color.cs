using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Assets
{
    /// <summary>
    /// Color data from 0.0F to 1.0F (this matches Unity's color object)
    /// </summary>
    [Serializable]
    public class Color
    {
        public float r;
        public float g;
        public float b;
        public float a;
    }
}
