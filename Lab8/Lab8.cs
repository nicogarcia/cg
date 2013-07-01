using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities.Shaders;
using Utilities.Shapes;
using System.IO;

namespace Lab8
{
	public partial class Lab8 : Form
	{
		Foot foot;
		Cover cover;
		Cylinder cylinder;
		ProgramObject program;

		public Lab8()
		{
			InitializeComponent();
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{

			program = new ProgramObject(
			   new VertexShader(Shaders.VERTEX_SHADER_TEXTURE),
				   new FragmentShader(Shaders.FRAGMENT_SHADER_TEXTURE));
			
			cylinder = new Cylinder(1, 4, 100, new Vector4(), program);

			cylinder.transformation = Matrix4.CreateTranslation(0, 0, -2);


			foot = new Foot(2f, 20, program);
			cover = new Cover(0.25f, 1, program);

			foot.transformation = Matrix4.CreateTranslation(0, 0, -1);
			cover.transformation = Matrix4.CreateTranslation(0, 0, -1);

			LoadImageTexture.LoadTexture(@"..\..\texture.jpg");

			openGLControl1.objects.Add(foot);
			openGLControl1.objects.Add(cover);
			
			openGLControl1.load();
		}

		private void Lab8_KeyUp(object sender, KeyEventArgs e)
		{
			if (e.KeyValue == (int)Keys.Escape)
				this.Dispose();

			openGLControl1.Invalidate();
		}

		private void btn_cylinder_Click(object sender, EventArgs e)
		{
			openGLControl1.objects.Clear();

			openGLControl1.objects.Add(cylinder);

			openGLControl1.Invalidate();
		}

		private void btn_table_Click(object sender, EventArgs e)
		{
			openGLControl1.objects.Clear();

			openGLControl1.objects.Add(foot);
			openGLControl1.objects.Add(cover);

			openGLControl1.Invalidate();
		}

		private void chk_draw_normals_CheckedChanged(object sender, EventArgs e)
		{
			bool state = ((CheckBox)sender).Checked;

			foot.draw_normals = state;
			cover.draw_normals = state;

			cylinder.draw_normals = state;

			openGLControl1.Invalidate();
		}

	}
}
