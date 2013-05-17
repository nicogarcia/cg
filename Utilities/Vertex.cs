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
        public Vector4 normal  = new Vector4(0, 0, 0, 1f);
        public Vector4 color = new Vector4(0, 0, 0, 1f);
        public Vector4 texture = new Vector4(0, 0, 0, 1f);

        public Vertex(Vector4 position)
        {
            this.position = position;
        }

        public Vertex(float x,float y,float z,float w)
        {
            position = new Vector4(x, y, z, w);
        }
        
        public Vector4[] ToArray()
        {
            return new Vector4[]{
                position,
                normal,
                color,
                texture
            };
        }

    }

    public class VertexArray
    {
        public static Vector4[] singleVector4Array(Vertex[] array){
            Vector4[] toReturn = new Vector4[array.Length * 4];

            for(int i = 0; i < array.Length; i++){
                toReturn[i * 4] = array[i].position;
                toReturn[i * 4 + 1] = array[i].normal;
                toReturn[i * 4 + 2] = array[i].color;
                toReturn[i * 4 + 3] = array[i].texture;
            }

            return toReturn;
        }
    }
}
