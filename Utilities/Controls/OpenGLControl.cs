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
        GhostCamera camera;
        GhostCamera lastCameraState;

        System.Timers.Timer refreshTimer = new System.Timers.Timer();

        private Matrix4 projMatrix;
        private Matrix4 zoomMatrix;

        bool[] pressed_keys = new bool[256];

        public OpenGLControl()
        {
            //camera = new Camera(new Spherical(8f, (float)Math.PI / 4,(float) Math.PI / 2));
            //lastCameraState = new Camera(new Spherical(8f, (float)Math.PI / 4, (float)Math.PI / 2));
            camera = new GhostCamera();
            lastCameraState = new GhostCamera();

            //projMatrix = Matrix4.CreateOrthographicOffCenter(-10f, 10f, -10f, 10f, 0.0001f, 10000f);
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.1f, this.Width / this.Height, 1f, 1000f);
            zoomMatrix = camera.lookAt();

            refreshTimer.Interval = 20;
            refreshTimer.Elapsed += new ElapsedEventHandler(refreshTimer_Elapsed);
            refreshTimer.Enabled = true;
        }

        void refreshTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            refreshView();
        }

        public void load()
        {
            GL.ClearColor(Color.Azure);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);

            GL.Viewport(0, 0, this.Width, this.Height);

            this.Paint += new PaintEventHandler(this.paint);
            this.Resize += new EventHandler(this.resize);
            this.MouseWheel += new MouseEventHandler(OpenGLControl_MouseWheel);
        }

        public void paint(object sender, PaintEventArgs e)
        {
            OpenGLControl control = (OpenGLControl)sender;

            GL.ActiveTexture(TextureUnit.Texture0);

            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.Clear(ClearBufferMask.DepthBufferBit);

            foreach (Drawable3D draw in objects)
            {
                draw.paint(projMatrix, zoomMatrix);
            }

            control.SwapBuffers();
        }

        public void resize(object sender, EventArgs e)
        {
            OpenGLControl control = (OpenGLControl)sender;

            projMatrix = Matrix4.CreatePerspectiveFieldOfView(0.1f, this.Width / (float)this.Height, 1f, 100f);

            GL.Viewport(0, 0, control.Width, control.Height);
            //GL.Viewport(control.Width - 100, control.Height - 100, 100, 100);

            control.Invalidate();
        }

        public void OpenGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                camera.moveBack();
            else
                camera.moveForward();

            zoomMatrix = camera.lookAt();
            this.Invalidate();
        }

        public void refreshView()
        {
            MotionControl.refreshCamera(camera);

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
    }
}
