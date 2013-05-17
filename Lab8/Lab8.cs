using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities.Shaders;

namespace Lab8
{
    public partial class Lab8 : Form
    {
        Foot sweep;
        Cylinder cylinder;

        public Lab8()
        {
            InitializeComponent();
        }

        private void openGLControl1_Load(object sender, EventArgs e)
        {
            ProgramObject program = new ProgramObject(
                new VertexShader(Shaders.VERTEX_SHADER_TEXTURE),
                    new FragmentShader(Shaders.FRAGMENT_SHADER_ILLUMINATION));

            // Constants
            int num = 100;
            float radius = 1.0f;
            cylinder = new Cylinder(radius, num, program);


            Vertex[] vertices = new Vertex[]{
                new Vertex(new Vector4(1f, -1f, 0, 1f)),
                new Vertex(new Vector4(-1f, -1f, 0, 1f)),
                new Vertex(new Vector4(-1f, 1f, 0, 1f)),
                new Vertex(new Vector4(1f, 1f, 0, 1f)),
            };

            sweep = new Foot(vertices,
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
                , 20, program);

            openGLControl1.objects.Add(cylinder);

            //cargo la textura
            Utilities.LoadImageTexture.LoadTexture(@"..\..\texture.jpg");
            
            openGLControl1.load();
        }

        private void figureDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            openGLControl1.objects.Clear();

            if (figureDropDown.SelectedItem.Equals("Mesa")) 
                openGLControl1.objects.Add(sweep);
            else
                openGLControl1.objects.Add(cylinder);

            openGLControl1.Invalidate();

        }


    }
}
