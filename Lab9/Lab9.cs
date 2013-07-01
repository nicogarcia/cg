using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using OpenTK;
using Utilities.Shaders;

namespace Lab9
{
	public partial class Lab9 : Form
	{
		Foot foot;
		Cover cover;
		Cylinder cylinder;
		ProgramObject program;

		public Lab9()
		{
			InitializeComponent();
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{
			program = new ProgramObject(
			   new VertexShader(Shaders.VERTEX_SHADER_TEXTURE),
				   new FragmentShader(Shaders.FRAGMENT_SHADER_ILLUMINATION));

			cylinder = new Cylinder(1, 4, 100, new Vector4(), program);

			cylinder.transformation = Matrix4.CreateTranslation(0, 0, -2);

			Light light = new Light(new Spherical(5, MathHelper.PiOver2, 0));

			foot = new Foot(2f, 20, program);
			foot.colored = false;
			foot.light = light;

			cover = new Cover(0.25f, 1, program);
			cover.noise_texture = true;
			cover.colored = false;
			cover.light = light;


			foot.transformation = Matrix4.CreateTranslation(0, 0, -1);
			cover.transformation = Matrix4.CreateTranslation(0, 0, -1);

			LoadImageTexture.LoadTexture(@"..\..\texture.jpg");

			openGLControl1.objects.Add(foot);
			openGLControl1.objects.Add(cover);

			openGLControl1.light = light;

			openGLControl1.load();
		}

		private void btn_nayar_Click(object sender, EventArgs e)
		{
			foot.illumination_model = 2;
			cover.illumination_model = 2;

			openGLControl1.Invalidate();
		}

		private void btn_cook_Click(object sender, EventArgs e)
		{
			foot.illumination_model = 1;
			cover.illumination_model = 1;

			openGLControl1.Invalidate();
		}

		private void btn_phong_Click(object sender, EventArgs e)
		{
			foot.illumination_model = 0;
			cover.illumination_model = 0;

			openGLControl1.Invalidate();
		}
	}
}
