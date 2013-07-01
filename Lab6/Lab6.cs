using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using Utilities.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Lab6
{
	public partial class Lab6 : Form
	{
		public Lab6()
		{
			InitializeComponent();
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{
			ProgramObject program = new ProgramObject(
			   new VertexShader(Shaders.VERTEX_TRANSFORMATION_SHADER),
				   new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

			WavefrontObj obj = new WavefrontObj(@"..\..\ashtray.obj", program, null);
			obj.transformation = Matrix4.Scale(0.15f) * Matrix4.CreateRotationX(MathHelper.PiOver2);

			openGLControl1.objects.Add(obj);

			openGLControl1.load();
		}

		
	}
}
