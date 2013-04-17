using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities
{
    public class Camera
    {
        Vector4 normal;
        Vector4 lookat;
        public Spherical position;

        public Camera(Spherical position)
        {
            this.normal = new Vector4(0, 0, 1f, 1f);
            this.position = position;
            this.lookat = new Vector4(0, 0, 0, 1f);
        }

        public Matrix4 lookAt()
        {
            return Matrix4.LookAt(position.x, position.y, position.z, lookat.X, lookat.Y, lookat.Z, normal.X, normal.Y, normal.Z);
        }

    }
}
