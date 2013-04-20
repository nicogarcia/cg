using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public abstract class Sweep
    {
        Vertex[] vertices;
        Matrix4 translation_step;
        Matrix4 rotation_step;
        Matrix4 scale_step;

        public Sweep(Vertex[] face_vertices, Vector4 translation, 
            float rotation_step, float scale_step, int steps)
        {

        }
    }
}
