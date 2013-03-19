using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_TP1.Shapes
{
    class Star : Drawable
    {
        public Star(float inner_radius, float outer_radius)
        {
            float PI = (float) Math.PI;
            float theta = PI / 2;
            float step = 2 * PI / 10f;

            vertices = new Vector4[10];
            float[] radii = { outer_radius, inner_radius };

            for (int i = 0; i < 10; i++, theta += step)
            {
                float radius = radii[i % 2];
                vertices[i] = new Vector4(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta), 0f, 1f);
            }
            GLManager.addDrawable(this, BeginMode.LineLoop);
        }
    }
}
