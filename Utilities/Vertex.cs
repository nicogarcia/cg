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
        public Vector4 color = new Vector4(1.0f, 0, 0, 1f);
        public Vector4 texture = new Vector4(0, 0, 0, 0);
        private Vector4 vector4;
        private Vector4 color_2;
        private Vector4 vector4_2;

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
        /*
        public Vertex(Vertex otherVertex, Vector4 texture)
        {
            position = otherVertex.position;
            normals = otherVertex.normals;
            color = otherVertex.color;
            this.textures.Add(texture);
        }*/

        public Vertex(float x,float y,float z,float w)
        {
            position = new Vector4(x, y, z, w);
        }
        
        public Vector4[] ToArray()
        {
            Vector4[] toret = new Vector4[4];

            toret[0] = position;
            toret[1] = normal;
            toret[2] = color;
            toret[3] = texture;

            return toret;
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
