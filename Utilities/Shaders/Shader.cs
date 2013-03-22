using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CG_TP1
{
    public abstract class Shader
    {
        public int shader_handle;
        public string shader_source;
        public bool compiled = false;

        abstract public void compileShader();
    }
}
