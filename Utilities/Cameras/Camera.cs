using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public abstract class Camera
    {
        public WavefrontObj car;

        public abstract Matrix4 lookAt();

        public abstract void refreshCamera();

        public abstract void moveBack();
        public abstract void moveForward();
    }
}
