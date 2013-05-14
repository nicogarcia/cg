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
        public Lab8()
        {
            InitializeComponent();
        }

        private void openGLControl1_Load(object sender, EventArgs e)
        {
            ProgramObject program = new ProgramObject(
                new VertexShader(Shaders.VERTEX_SHADER_LATEST),
                    new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

            // Constants
            int num = 10;
            float radius = 1.0f;
            Cylinder cylinder = new Cylinder(radius, num, program);

            openGLControl1.objects.Add(cylinder);

            openGLControl1.load();
        }
    }
}
