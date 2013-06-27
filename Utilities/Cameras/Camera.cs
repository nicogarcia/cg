using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public abstract class Camera
    {
        public abstract Matrix4 lookAt();
	}
}
