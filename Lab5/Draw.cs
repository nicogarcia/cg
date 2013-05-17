using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using Utilities.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Lab5
{
    public class Draw : Drawable3D
    {
        public Draw(ProgramObject program, BeginMode begin_mode)
            : base(program, begin_mode)
        {
            /*Triangle t = new Triangle(new Vertex[]{
                new Vertex(new Vector4(0, 0, 0, 1f)),
                new Vertex(new Vector4(0.5f, 0.5f, 0, 1f)),
                new Vertex(new Vector4(0.5f, 0, 0, 1f)),
            });

            triangles.Add(t);*/


            //triangles.triangulate(sweep.polynet.faces.ToArray());
            
        }
    }
}
