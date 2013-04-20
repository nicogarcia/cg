using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public abstract class Drawable2D
    {
        public Vector4[] vertices;
        public Matrix4 transformation;

        public void paint()
        {
            GLManager.paintDrawable(this);
        }
    }
}