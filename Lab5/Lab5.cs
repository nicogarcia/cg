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
    public partial class Lab5 : Form
    {
        public Lab5()
        {
            InitializeComponent();

        }

        private void OpenGLcontrol_load(object sender, EventArgs e)
        {
            ProgramObject program = new ProgramObject(
                new VertexShader(Shaders.VERTEX_TRANSFORMATION_SHADER),
                    new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

			Foot foot = new Foot(2f, 20, program);
			Cover cover = new Cover(0.5f, 1, program);

            openGLControl1.objects.Add(foot);
            openGLControl1.objects.Add(cover);
            
            foot.transformation = Matrix4.CreateTranslation(0, 0,-1);
            cover.transformation = Matrix4.CreateTranslation(0, 0, -1);

			foot.wire_mode = true;
			cover.wire_mode = true;

            openGLControl1.load();
        }

    }
}
