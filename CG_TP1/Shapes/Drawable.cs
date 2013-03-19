using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace CG_TP1
{
    public abstract class Drawable
    {
        public Vector4[] vertices;
        public void paint()
        {
            GLManager.paintDrawable(this);
        }
    }
}
