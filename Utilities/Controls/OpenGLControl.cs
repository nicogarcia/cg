using System;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Utilities
{
    public class OpenGLControl : GLControl
    {
        public List<Drawable3D> objects = new List<Drawable3D>();
        Camera camera;

        private Matrix4 projMatrix;
        private Matrix4 zoomMatrix;

        public OpenGLControl()
        {
            camera = new Camera(new Spherical(8f, (float)Math.PI / 4,(float) Math.PI / 2));
            //camera = new GhostCamera();

            //projMatrix = Matrix4.CreateOrthographicOffCenter(-10f, 10f, -10f, 10f, 0.0001f, 10000f);
            projMatrix = Matrix4.CreatePerspectiveFieldOfView(1f, this.Width / this.Height, 1f, 100f);
            zoomMatrix = camera.lookAt();

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
            this.KeyPress += new KeyPressEventHandler(OpenGLControl_KeyPress);
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

            projMatrix = Matrix4.CreatePerspectiveFieldOfView(1f, this.Width / (float) this.Height, 1f, 100f);

            GL.Viewport(0, 0, control.Width, control.Height);

            control.Invalidate();
        }

        public void OpenGLControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                camera.position.growRadio();
            else
                camera.position.shrinkRadio();

            zoomMatrix = camera.lookAt();
            this.Invalidate();
        }

        public void OpenGLControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'w':
                    camera.position.growTheta();
                    break;
                case 's':
                    camera.position.shrinkTheta();
                    break;
                case 'd':
                    camera.position.growPhi();
                    break;
                case 'a':
                    camera.position.shrinkPhi();
                    break;
                case '-':
                    camera.position.growRadio();
                    break;
                case '+':
                    camera.position.shrinkRadio();
                    break;
            }

            zoomMatrix = camera.lookAt();
            this.Invalidate();
        }
    }
}
