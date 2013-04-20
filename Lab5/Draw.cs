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

            Vector4[] vertices = new Vector4[]{
                new Vector4(0.5f, -0.5f, 0, 1f),
                new Vector4(-0.5f, -0.5f, 0, 1f),
                new Vector4(-0.5f, 0.5f, 0, 1f),
                new Vector4(0.5f, 0.5f, 0, 1f),
            };

            NewFoot sweep = new NewFoot(vertices,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0, 0, 1f));
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
                        return Matrix4.Scale(0.1f);
                    }
                )
                , 20);

            triangles.triangulate(sweep.polynet.faces.ToArray());

            reloadTriangles();
        }
    }
}
