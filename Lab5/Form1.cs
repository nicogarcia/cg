using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;
using Utilities;
using OpenTK;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

        }

        private void OpenGLcontrol_load(object sender, EventArgs e)
        {
            ProgramObject prog = new ProgramObject(
                new VertexShader(Shaders.VERTEX_TRANSF_SHADER),
                    new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

            //Draw draw = new Draw(prog, BeginMode.LineLoop);

            /*Vertex[] vertices = new Vertex[]{
                new Vertex(new Vector4(0.5f, -0.5f, 0, 1f)),
                new Vertex(new Vector4(-0.5f, -0.5f, 0, 1f)),
                new Vertex(new Vector4(-0.5f, 0.5f, 0, 1f)),
                new Vertex(new Vector4(0.5f, 0.5f, 0, 1f)),
            };*/


            Vertex[] vertices = new Vertex[]{
                new Vertex(1f, -1f, 0, 1f),
                new Vertex(-1f, -1f, 0, 1f),
                new Vertex(-1f, 1f, 0, 1f),
                new Vertex(1f, 1f, 0, 1f),
            };

            /*Foot sweep = new Sweep(vertices,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0, 0, (current + 1) * 0.1f));
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
                        current++;

                        float pend = 4 * 0.5f / (float)Math.Pow(steps * 0.1f, 2);
                        float scale_factor = (float)(pend * Math.Pow(current*0.1f - steps*0.1f / 2f, 2) + 0.5f);
                        return Matrix4.Scale(scale_factor, scale_factor, 1f);
                    }
                )
                , 50, prog);

            float x = 2.0f;
            float y = 2.0f;
            float z = 5f;
            float aux = 0.15f;

            Vertex[] bottom = new Vertex[]{
               new Vertex(-x, -y + aux, z, 1.0f),
               new Vertex(-x + aux, -y, z, 1.0f),
               new Vertex(x - aux, -y, z, 1.0f),
               new Vertex(x, -y + aux, z, 1.0f),
               new Vertex(x, y - aux, z, 1.0f),
               new Vertex(x - aux, y, z, 1.0f),
               new Vertex(-x + aux, y, z, 1.0f),
               new Vertex(-x, y - aux, z, 1.0f)
            };


            Foot sweep2 = new Foot(bottom,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0, 0, (current + 1) * 0.1f));
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
                , 5, prog);

            openGLControl1.objects.Add(sweep);
            openGLControl1.objects.Add(sweep2);
            
            sweep.transformation = Matrix4.CreateTranslation(0, -0.65f, 0);
            sweep2.transformation = Matrix4.CreateTranslation(0, -0.65f, 0);

            openGLControl1.load();*/
        }

    }
}
