using System;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Timers;

namespace Utilities
{
	public class OpenGLControl : GLControl
	{
		public List<Drawable3D> objects = new List<Drawable3D>();
		Camera camera;

		public Rectangle default_viewport_size;
		public List<Viewport> viewports = new List<Viewport>();

		System.Timers.Timer refreshTimer = new System.Timers.Timer();
		public Light light = new Light(new Spherical(5, MathHelper.PiOver2, 0));

		public Matrix4 projMatrix;
		public Matrix4 zoomMatrix;

		public enum CAMERA_MODE
		{
			CAR_CAMERA, SPHERICAL_CAMERA
		};
		public CAMERA_MODE camera_mode = CAMERA_MODE.SPHERICAL_CAMERA;

		bool[] pressed_keys = new bool[256];

		public OpenGLControl()
		{
			camera = new SphericalCamera(new Spherical(8, (float)Math.PI / 2, 0));

			projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.7f, this.Width / this.Height, 0.15f, 100f);
			zoomMatrix = camera.lookAt();

			refreshTimer.Interval = 20;
			refreshTimer.Elapsed += new ElapsedEventHandler(refreshTimer_Elapsed);
		}

		public void enable_timer()
		{
			refreshTimer.Enabled = true;
		}
		void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
		{
			refreshView();
		}

		public void load()
		{
			GL.ClearColor(Color.LightBlue);
			GL.Enable(EnableCap.DepthTest);
			GL.Enable(EnableCap.Texture2D);

			// Ignore materials with alpha > 0.5
			/*GL.Enable(EnableCap.AlphaTest);
			GL.AlphaFunc(AlphaFunction.Greater, 0.5f);

			GL.Enable(EnableCap.Blend);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
			*/
			enable_timer();

			this.Paint += new PaintEventHandler(this.paint);
			this.MouseWheel += new MouseEventHandler(OpenGLControl_MouseWheel);
		}

		public void paint(object sender, PaintEventArgs e)
		{
			OpenGLControl control = (OpenGLControl)sender;

			GL.ActiveTexture(TextureUnit.Texture0);

			GL.Clear(ClearBufferMask.ColorBufferBit);
			GL.Clear(ClearBufferMask.DepthBufferBit);

			if (default_viewport_size.Equals(new Rectangle()))
				GL.Viewport(0, 0, this.Width, this.Height);
			else
			{
				GL.Viewport(default_viewport_size);
				projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.7f, (float)default_viewport_size.Width / default_viewport_size.Height, 0.15f, 100f);
			}

			foreach (Drawable3D draw in objects)
			{
				draw.paint(projMatrix, zoomMatrix);
			}


			foreach (Viewport viewport in viewports)
			{
				GL.Viewport(viewport.x, viewport.y, viewport.width, viewport.height);

				GL.Clear(ClearBufferMask.DepthBufferBit);

				viewport.paint();
			}

			control.SwapBuffers();
		}

		public void AddViewport(Viewport viewport)
		{
			// If the viewport doesn't have matrix, add this' by default
			if (!viewport.matrix_set)
				viewport.setMatrices(projMatrix, zoomMatrix);

			viewports.Add(viewport);

		}

		public void OpenGLControl_MouseWheel(object sender, MouseEventArgs e)
		{
			if (camera_mode == CAMERA_MODE.SPHERICAL_CAMERA)
				if (e.Delta < 0)
					((SphericalCamera)camera).moveBack();
				else
					((SphericalCamera)camera).moveForward();

			zoomMatrix = camera.lookAt();
			this.Invalidate();
		}

		public void setCarCamera(CarCamera cam)
		{
			camera = cam;
			camera_mode = CAMERA_MODE.CAR_CAMERA;

			refreshView();
		}

		public void setSphericalCamera(SphericalCamera cam)
		{
			camera = cam;
			camera_mode = CAMERA_MODE.SPHERICAL_CAMERA;

			refreshView();
		}

		public void refreshView()
		{
			if (camera_mode == CAMERA_MODE.CAR_CAMERA)
				MotionControl.refreshCamera();
			else
				MotionControl.refreshCamera((SphericalCamera)camera);

			MotionControl.refreshLight(light);

			zoomMatrix = camera.lookAt();
			this.Invalidate();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);
			MotionControl.keyDown(e.KeyValue);
		}

		protected override void OnKeyUp(KeyEventArgs e)
		{
			base.OnKeyUp(e);
			MotionControl.keyUp(e.KeyValue);
		}

		private void InitializeComponent()
		{
			this.SuspendLayout();
			// 
			// OpenGLControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.Name = "OpenGLControl";
			this.ResumeLayout(false);

		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			this.Focus();
		}

		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			if (IsHandleCreated)
				if (default_viewport_size.Equals(new Rectangle()))
					projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.7f, AspectRatio, 0.15f, 100f);
				else
					projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.7f, (float)default_viewport_size.Width / default_viewport_size.Height, 0.15f, 100f);

		}

	}
}
