using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities
{
    public abstract class Drawable
    {
        public int VAO_ID;
        public int VBO_ID;

        public TriangleCollection triangles;
        public Matrix4 transformation;

        public BeginMode begin_mode;

        public void paint()
        {
            // Draw triangles
        }
    }
}
