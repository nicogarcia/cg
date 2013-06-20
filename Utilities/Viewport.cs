using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public class Viewport
    {
        public int x;
        public int y;
        public int width;
        public int height;
        public List<Drawable3D> objects = new List<Drawable3D>();

        Matrix4 projMatrix;
        Matrix4 zoomMatrix;

        public bool matrix_set = false;

        public Viewport(int x, int y, int width, int height)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public void setMatrices(Matrix4 projMatrix, Matrix4 zoomMatrix)
        {
            this.projMatrix = projMatrix;
            this.zoomMatrix = zoomMatrix;
            matrix_set = true;
        }

        public void AddObject(Drawable3D Object)
        {
            objects.Add(Object);
        }

        public void AddObjects(List<Drawable3D> Objects)
        {
            objects.AddRange(Objects);
        }

        public void paint()
        {
            foreach (Drawable3D draw in objects)
            {
                draw.paint(projMatrix, zoomMatrix);
            }
        }
    }
}
