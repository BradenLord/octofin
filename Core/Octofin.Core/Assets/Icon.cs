using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Octofin.Core.Assets
{
    [Serializable]
    public class Icon
    {
        public static readonly string DefaultIconPath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Assets" + Path.DirectorySeparatorChar + "Icons" + Path.DirectorySeparatorChar;

        public readonly string path;
        public Color color;

        public Icon(String path)
        {
            this.path = path;
        }
    }
}
