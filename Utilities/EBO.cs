using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public class EBO
    {
        public int firstIndex { get; set; }
        public int lastIndex { get; set; }
        public int[] indices { get; set; }
        public int id { get; set; }
        public Material material { get; set; }
        public Matrix4 transformation { get; set; }
        public string Name { get; set; }

    }
}
