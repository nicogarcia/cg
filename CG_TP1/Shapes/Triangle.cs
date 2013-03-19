using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_TP1
{
    class Triangle : Drawable
    {
        public Triangle(float Base, float Height, PointF center)
        {
            float halfBase = Base / 2f;
            float halfHeight = Height / 2f;

            vertices = new Vector4[]{
                new Vector4(center.X, halfHeight + center.Y, 0f, 1f),
                new Vector4(-halfBase + center.X, -halfHeight + center.Y, 0f, 1f),
                new Vector4(halfBase + center.X, -halfHeight + center.Y, 0f, 1f),
            };

            GLManager.addDrawable(this, BeginMode.Triangles);
        }

    }
}
