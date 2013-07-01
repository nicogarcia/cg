using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using Utilities;
using Utilities.Shapes;
using Utilities.Shaders;

namespace Lab1
{
	public partial class Lab1 : Form
	{
		private Drawable2D[] figures;

		public Lab1()
		{
			InitializeComponent();
		}

		private void glControl_Load(object sender, EventArgs e)
		{
			GL.ClearColor(Color.Yellow);


			figures = new Drawable2D[] { new Sierpinski(new OpenTK.Vector4[]{
                        new OpenTK.Vector4(-.75f, -.75f, 0f, 1f),
                        new OpenTK.Vector4(0, .75f, 0f, 1f),
                        new OpenTK.Vector4(.75f, -.75f, 0f, 1f)
                    })};
		}

		private void glControl_Paint(object sender, PaintEventArgs e)
		{
			GL.Clear(ClearBufferMask.ColorBufferBit);

			if (figures != null)
				for (int i = 0; i < figures.Length; i++)
				{
					figures[i].paint();
				}

			openGLControl1.SwapBuffers();
		}

		private void glControl_Resize(object sender, EventArgs e)
		{
			int aspect_y = 1, aspect_x = 1;

			// Calculate resize ratios for resizing
			int ratioW = openGLControl1.Width / aspect_x;
			int ratioH = openGLControl1.Height / aspect_y;

			// smaller ratio will ensure that the image fits in the view
			int ratio = ratioW < ratioH ? ratioW : ratioH;

			GL.Viewport((openGLControl1.Width - aspect_x * ratio) / 2, (openGLControl1.Height - aspect_y * ratio) / 2,
				aspect_x * ratio, aspect_y * ratio);

			openGLControl1.Invalidate();
		}
	}
}