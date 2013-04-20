using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;

namespace Lab5
{
    public class NewFoot : Sweep
    {

        public NewFoot(Vector4[] face_vertices, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step, int steps)
            : base(face_vertices, translation_step, rotation_step, scale_step, steps)
        {

        }
    }
}
