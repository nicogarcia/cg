using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities.Shaders;
using Utilities;
using OpenTK;

namespace Lab7
{
	public partial class Lab7 : Form
	{
		WavefrontObj obj;

		public Lab7()
		{
			InitializeComponent();
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{
			ProgramObject program = new ProgramObject(
			   new VertexShader(Shaders.VERTEX_SHADER_TEXTURE),
				   new FragmentShader(Shaders.FRAGMENT_SHADER_ILLUMINATION));

			Light light = new Light(new Spherical(5, MathHelper.PiOver2, 0));

			obj = new WavefrontObj(@"..\..\ashtray.obj", program, null);
			obj.transformation = Matrix4.Scale(0.15f) * Matrix4.CreateRotationX(MathHelper.PiOver2);
			obj.light = light;

			LoadImageTexture.LoadTexture(@"..\..\texture.jpg");

			openGLControl1.objects.Add(obj);
			openGLControl1.light = light;

			openGLControl1.load();

		}

		private void btn_nayar_Click(object sender, EventArgs e)
		{
			obj.illumination_model = 2;

			openGLControl1.Invalidate();
		}

		private void btn_cook_Click(object sender, EventArgs e)
		{
			obj.illumination_model = 1;

			openGLControl1.Invalidate();
		}

		private void btn_phong_Click(object sender, EventArgs e)
		{
			obj.illumination_model = 0;

			openGLControl1.Invalidate();
		}


	}
}
