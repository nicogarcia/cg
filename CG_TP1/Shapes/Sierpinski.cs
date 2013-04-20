using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CG_TP1.Shapes;
using Utilities;
using Utilities.Shaders;

namespace CG_TP1
{
    class Sierpinski : Drawable2D
    {
        const int ITERATIONS = 1 << 20;
       
        public Sierpinski(Vector4[] vertices_i)
        {
            vertices = new Vector4[ITERATIONS];
            Random rand = new Random();
            Vector4 Pk = vertices_i[rand.Next() % 3];

            int i = 0;
            while (i != ITERATIONS)
            {
                Vector4 Pt = vertices_i[rand.Next() % 3];
                Vector4 Pk1 = Pk;
                Pk = new Vector4(Pt.X + (Pk1.X - Pt.X) / 2f, Pt.Y + (Pk1.Y - Pt.Y) / 2f, 0f, 1f);

                vertices[i++] = Pk;
            }

            GLManager.addDrawable(this, BeginMode.Points, ProgramObject.DEFAULT_PROGRAM);
        }
    }
}
