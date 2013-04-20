using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Triangle
    {
        Vertex[] vertices;

        public Triangle(Vertex[] vertices)
        {
            this.vertices = new Vertex[3];

            for (int i = 0; i < vertices.Length; i++)
                this.vertices[i] = vertices[i];
        }

        public Vertex[] getArray()
        {
            return vertices;
        }
    }
}
