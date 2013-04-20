using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public class TriangleCollection : List<Triangle>
    {

        public Vector4[] getArray()
        {
            Vector4[] toRet = new Vector4[this.Count * 3 * 3];

            int cursor = 0;
            foreach (Triangle triangle in this)
            {
                Vertex[] vertices = triangle.getArray();

                for (int i = 0; i < 3; i++)
                {
                    toRet[cursor++] = vertices[i].position;
                    toRet[cursor++] = vertices[i].color;
                    toRet[cursor++] = vertices[i].normal;
                }
            }

            return toRet;
        }

        public void triangulate(Face[] faces)
        {
            throw new NotImplementedException();
        }
    }
}
