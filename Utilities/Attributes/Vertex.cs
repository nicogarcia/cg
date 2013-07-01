using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public class Vertex
    {
        public Vector4 position = new Vector4(0, 0, 0, 1f);
        public Vector4 normal = new Vector4(0, 0, 0, 1f);
        public Vector4 color = new Vector4(0.5f, 0.5f, 0.5f, 1f);
        public Vector4 texture = new Vector4(0, 0, 0, 0);

        public Vertex(Vector4 position)
        {
            this.position = position;
        }
        public Vertex(Vector4 position, Vector4 color)
        {
            this.color = color;
            this.position = position;
        }

        public Vertex(Vector4 position, Vector4 color, Vector4 texture)
        {
            this.position = position;
            this.color = color;
            this.texture = texture;
        }

        public Vertex(Vector4 position, Vector4 normal, Vector4 color, Vector4 texture)
        {
            this.position = position;
            this.color = color;
            this.texture = texture;
            this.normal = normal;
        }

        public Vertex(float x,float y,float z,float w)
        {
            position = new Vector4(x, y, z, w);
        }

        public Vertex(Vertex vertex)
        {
            this.position = vertex.position;
            this.color = vertex.color;
            this.texture = vertex.texture;
            this.normal = vertex.normal;
        }
        
    }

    public class VertexArray
    {
        public static Vector4[] singleVector4Array(Vertex[] array){

            Vector4[] toRet = new Vector4[4 * array.Length];
            int count = array.Length;

            for (int i = 0; i < array.Length; i++)
            {
                toRet[i] = array[i].position;
                toRet[i + count] = array[i].normal;
                toRet[i + count * 2] = array[i].color;
                toRet[i + count * 3] = array[i].texture;
            }

            return toRet;
        }
    }
}
