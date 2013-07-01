using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities.Shaders;
using Tao.FreeGlut;
using Tao.OpenGl;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities;

namespace Lab4
{
	public partial class Lab4 : Form
	{
		public Lab4()
		{
			InitializeComponent();
		}

		private void openGLControl1_Load(object sender, EventArgs e)
		{
			ProgramObject program = new ProgramObject(
				new VertexShader(Shaders.VERTEX_TRANSFORMATION_SHADER),
					new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

			Glut.glutInit();

			// Cubo Grande
			GlutDraw big_cube = new GlutDraw(program, () => { Glut.glutWireCube(5f); });
			big_cube.transformation = Translation(2.5f, 2.5f, 2.5f);

			// Cono X
			GlutDraw cone_x = new GlutDraw(program, () => { Glut.glutWireCone(0.5, 0.75, 10, 10); });
			cone_x.transformation = Matrix4.CreateRotationY(MathHelper.PiOver2) * Translation(2.5f, 0,0);

			// Cono Y
			GlutDraw cone_y = new GlutDraw(program, () => { Glut.glutWireCone(0.5, 0.75, 10, 10); });
			cone_y.transformation = Matrix4.CreateRotationX(-MathHelper.PiOver2) * Translation(0,2.5f, 0);

			// Cono Z
			GlutDraw cone_z = new GlutDraw(program, () => { Glut.glutWireCone(0.5, 0.75, 10, 10); });
			cone_z.transformation = Translation(0, 0, 2.5f);
			
			// Tetera
			GlutDraw teapot = new GlutDraw(program, () => { Glut.glutWireTeapot(1); });
			teapot.transformation = Matrix4.Scale(0.5f) * Matrix4.CreateRotationZ(MathHelper.PiOver2) * Translation(5, 5, 5);

			// Esfera
			GlutDraw sphere = new GlutDraw(program, () => { Glut.glutWireSphere(1f, 10, 10); });
			sphere.transformation = Matrix4.CreateRotationY(MathHelper.PiOver2) * Translation(0, 5, 5);

			// Cono
			GlutDraw cone = new GlutDraw(program, () => { Glut.glutWireCone(1, 2, 10, 10); });
			cone.transformation = Matrix4.CreateRotationY(MathHelper.PiOver2) * Translation(5,5,0);

			// Cubo Chico
			GlutDraw small_cube = new GlutDraw(program, () => { Glut.glutWireCube(1); });
			small_cube.transformation = Translation(5, 0, 5);

			// Cilindro 
			GlutDraw cylinder = new GlutDraw(program, () => { Glut.glutWireCylinder(0.5, 2, 10, 10); });
			cylinder.transformation = Matrix4.CreateRotationY(MathHelper.PiOver2) * Translation(5, 0, 0);
	
			// Toroide
			GlutDraw torus = new GlutDraw(program, () => { Glut.glutWireTorus(0.5, 0.75, 10, 10); });
			torus.transformation = Translation(0, 0, 5);

			// Dodecaedro
			GlutDraw dodecaedron = new GlutDraw(program, () => { Glut.glutWireDodecahedron(); });
			dodecaedron.transformation = Translation(0, 5, 0);

			openGLControl1.objects.Add(big_cube);
			openGLControl1.objects.Add(cone_x);
			openGLControl1.objects.Add(cone_y);
			openGLControl1.objects.Add(cone_z);
			openGLControl1.objects.Add(teapot);
			openGLControl1.objects.Add(sphere);
			openGLControl1.objects.Add(cone);
			openGLControl1.objects.Add(small_cube);
			openGLControl1.objects.Add(cylinder);
			openGLControl1.objects.Add(torus);
			openGLControl1.objects.Add(dodecaedron);

			openGLControl1.setSphericalCamera(new SphericalCamera(new Spherical(15, MathHelper.PiOver3, MathHelper.PiOver6)));

			openGLControl1.load();
		}

		private Matrix4 Translation(float x, float y, float z)
		{
			return Matrix4.CreateTranslation(x, y, z);
		}

		private void openGLControl1_Layout(object sender, LayoutEventArgs e)
		{
			openGLControl1.viewports.Clear();

			int w = openGLControl1.Width;
			int h = openGLControl1.Height;

			openGLControl1.default_viewport_size = new Rectangle(0, 0, 2 * w / 3, h);

			Matrix4 projMatrix = Matrix4.CreateOrthographicOffCenter(-10, 10, -10, 10, 0.5f, 100);

			Viewport up = new Viewport(2 * w / 3, 2 * h / 3, w / 3, h / 3);
			up.objects.AddRange(openGLControl1.objects);
			Matrix4 zoomMatrix = Matrix4.LookAt(new Vector3(0, 0, 10), new Vector3(0, 0.1f, 0), new Vector3(0, 0, 1));

			up.setMatrices(projMatrix, zoomMatrix);

			openGLControl1.AddViewport(up);


			Viewport front = new Viewport(2 * w / 3, h / 3, w / 3, h / 3);
			front.objects.AddRange(openGLControl1.objects);

			zoomMatrix = Matrix4.LookAt(new Vector3(10, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 1));

			front.setMatrices(projMatrix, zoomMatrix);

			openGLControl1.AddViewport(front);


			Viewport right = new Viewport(2 * w / 3, 0, w / 3, h / 3);
			right.objects.AddRange(openGLControl1.objects);

			zoomMatrix = Matrix4.LookAt(new Vector3(0, 10, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 1));

			right.setMatrices(projMatrix, zoomMatrix);

			openGLControl1.AddViewport(right);


			openGLControl1.Invalidate();
		}


	}
}
