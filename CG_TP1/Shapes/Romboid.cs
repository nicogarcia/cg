using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities;
using Utilities.Shaders;

namespace CG_TP1.Shapes
{
    class Romboid : Drawable2D
    {
        public Romboid(float width, float height, Matrix4 transformation)
        {
            this.transformation = transformation;

            vertices = new Vector4[8];

            vertices[0] = new Vector4(-width * 1 / 3, 0f, 0f, 1f);
            vertices[1] = new Vector4(0f, height / 2, 0f, 1f);
            vertices[2] = new Vector4(width * 2 / 3, 0f, 0f, 1f);
            vertices[3] = new Vector4(0f, -height / 2, 0f, 1f);

            vertices[4] = new Vector4(1f, 0f, 0f, 1f);
            vertices[5] = new Vector4(0f, 0f, 1f, 1f);
            vertices[6] = new Vector4(1f, 1f, 1f, 1f);
            vertices[7] = new Vector4(0f, 1f, 0f, 1f);

            GLManager.addDrawable(this, BeginMode.TriangleFan, new ProgramObject(new VertexShader(Shaders.VERTEX_TRANSF_SHADER),
                new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER)));

        }
    }
}
