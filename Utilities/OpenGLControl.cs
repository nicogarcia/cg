using System;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

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
            camera = new Camera(new Spherical(5f, 0, (float) Math.PI / 4));

            projMatrix = Matrix4.Identity;
            zoomMatrix = Matrix4.Identity;

        }

        public void load()
        {
            GL.ClearColor(Color.Yellow);
        }

        public void paint()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            
            foreach(Drawable3D draw in objects){
                draw.paint(projMatrix, zoomMatrix);
            }

            GL.Viewport(0, 0, this.Width, this.Height);

            this.SwapBuffers();
        }
    }
}
