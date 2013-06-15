using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    // Unused
    public class TriangleCollection : List<Triangle>
    {

        public Vector4[] getArray()
        {
            Vector4[] toRet = new Vector4[this.Count * 3 * 3];

            int cursor = 0;
            foreach (Triangle triangle in this)
            {
                Vertex[] vertices = triangle.getArray();
                int count = this.Count;

                for (int i = 0; i < 3; i++)
                {
                    toRet[cursor] = vertices[i].position;
                    toRet[cursor + count] = vertices[i].color;
                    //toRet[cursor + 2 * count] = vertices[i].normal;
                    cursor++;
                }
            }

            return toRet;
        }

        public void triangulate(Face[] faces)
        {
            foreach (Face f in faces)
            {
                Vertex[] vertices = f.vertices();
                for(int i = 0; i < vertices.Length - 2; i++)
                {
                    Triangle t = new Triangle(new Vertex[]{
                        new Vertex(vertices[0].position),
                        new Vertex(vertices[i + 1].position),
                        new Vertex(vertices[i + 2].position),
                    });
                    Add(t);
                }

            }
        }
    }
}
