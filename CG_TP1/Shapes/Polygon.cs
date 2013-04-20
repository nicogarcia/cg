using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities;
using Utilities.Shaders;

namespace CG_TP1.Shapes
{
    class Polygon : Drawable2D
    {

        public Polygon(Vector4[] vertices, bool solid)
        {
            this.vertices = vertices;

            if (solid)
                GLManager.addDrawable(this, BeginMode.Polygon, ProgramObject.DEFAULT_PROGRAM);
            else
                GLManager.addDrawable(this, BeginMode.LineStrip, ProgramObject.DEFAULT_PROGRAM);
        }
    }
}
