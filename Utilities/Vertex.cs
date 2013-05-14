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
        public Vector4 color = new Vector4(0, 0, 0, 1f);
        public Vector4 normal  = new Vector4(0, 0, 0, 1f);
        public Vector2 texture = new Vector2(0, 0);

        public Vertex(Vector4 position)
        {
            this.position = position;
        }

        /*public Vector4[] getVertex()
        {
            return new Vector4[]{
                position,
                color,
                normal,
                texture
            };
        }*/

    }
}
