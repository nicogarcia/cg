using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class TriangleCollection
    {
        List<Triangle> triangles = new List<Triangle>();

        public Triangle[] getArray()
        {
            return triangles.ToArray();
        }

        public void triangulize(Face[] faces)
        {
            throw new NotImplementedException();
        }
    }
}
