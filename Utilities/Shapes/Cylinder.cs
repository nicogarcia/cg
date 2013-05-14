using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Utilities.Shaders;

namespace Utilities
{
    public class Cylinder : Sweep
    {
        public Cylinder(float radius, int faces, ProgramObject program) : base(1, program)
        {
            float step = 2 * (float)Math.PI / faces;
            float theta = 0;

            Vector4[] base_vertices = new Vector4[faces];

            for (int i = 0; i < faces; i++, theta += step)
                base_vertices[i] = new Vector4(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta), -radius * 2, 1f);

            createSweep(
                base_vertices,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0f, 0f, radius * 4f));
                    }
                ),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.Identity;
                    }
                ),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.Identity;
                    }
                )
             );
        }

    }
}
