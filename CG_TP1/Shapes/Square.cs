using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CG_TP1.Shapes;

namespace CG_TP1
{
    class Square : Drawable
    {   

        public Square()
        {
            vertices = new Vector4[]
            {
                new Vector4(-0.5f, -0.5f, 0f, 1f),
                new Vector4(-0.5f, 0.5f, 0f, 1f),
                new Vector4(0.5f, 0.5f, 0f, 1f),
                new Vector4(0.5f, -0.5f, 0f, 1f),
            };

            GLManager.addDrawable(this, BeginMode.Quads, ProgramObject.DEFAULT_PROGRAM);
        }

    }
}
