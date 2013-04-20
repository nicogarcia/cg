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
    class Cross : Drawable2D
    {
        public Cross(float width, float height)
        {
            vertices = new Vector4[12];

            float big_step = (float)Math.Atan2(width / 6, height / 2) * 2f;
            float small_step = (float)Math.PI / 4f - big_step / 2f;

            float big_radius = (float)Math.Sqrt(Math.Pow(width / 6, 2) + Math.Pow(height / 2, 2));
            float small_radius = (float)Math.Sqrt(2 * Math.Pow(width / 6, 2));

            float[] steps = { big_step, small_step, small_step};
            float[] radii = { big_radius, small_radius, big_radius};

            float theta = big_step / 2f;
            float radius = big_radius;

            for (int i = 0; i < 12; i++, theta += steps[i % 3], radius = radii[i % 3])
                vertices[i] = new Vector4(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta), 0f, 1f);

            GLManager.addDrawable(this, BeginMode.LineLoop, ProgramObject.DEFAULT_PROGRAM);

        }
    }
}
