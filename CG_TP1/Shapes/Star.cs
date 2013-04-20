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
    class Star : Drawable2D
    {
        public Star(float inner_radius, float outer_radius)
        {
            float PI = (float)Math.PI;
            float theta = PI / 2;
            float step = 2 * PI / 10f;

            Random rand = new Random();

            vertices = new Vector4[10];
            float[] radii = { outer_radius, inner_radius };

            for (int i = 0; i < 10; i++, theta += step)
            {
                float radius = radii[i % 2];
                // Position
                vertices[i] = new Vector4(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta), 0f, 1f);
                // Color
                //vertices[i + 10] = new Vector4((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), 1f);
            }

            GLManager.addDrawable(this, BeginMode.LineLoop, ProgramObject.DEFAULT_PROGRAM);
        }
    }
}
