using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities.Shaders
{
    public class FragmentShader : Shader
    {
        public FragmentShader(string shader_source)
        {
            this.shader_source = shader_source;
            shader_handle = GL.CreateShader(ShaderType.FragmentShader);
        }

        public override void compileShader()
        {
            GL.ShaderSource(shader_handle, shader_source);

            GL.CompileShader(shader_handle);

            string info;
            GL.GetShaderInfoLog(shader_handle, out info);
            Console.WriteLine("Compiling fragment shader... {0}", info);

            compiled = true;
        }
    }
}
