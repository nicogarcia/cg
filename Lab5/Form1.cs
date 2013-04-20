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
                new VertexShader(Shaders.VERTEX_SHADER_LATEST),
                    new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

            Draw draw = new Draw(prog, BeginMode.LineLoop);

            openGLControl1.objects.Add(draw);

            openGLControl1.load();
        }

        private void OpenGLcontrol_paint(object sender, PaintEventArgs e)
        {

            GL.ClearColor(Color.Azure);
            openGLControl1.paint();
        }
    }
}
