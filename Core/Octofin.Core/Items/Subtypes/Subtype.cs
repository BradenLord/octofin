using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Octofin.Core.Items.Subtypes
{
    [Serializable]
    public class Subtype
    {
        public readonly string label;

        public Subtype(string label)
        {
            this.label = label;
        }

        public override string ToString()
        {
            return label;
        }
    }
}
