using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities.Shaders
{
    public class ProgramObject
    {
        public static ProgramObject DEFAULT_PROGRAM = 
            new ProgramObject(new VertexShader(Shaders.DEFAULT_VERTEX_SHADER),
                new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));

        public int program_handle;
        public VertexShader vertex_shader;
        public FragmentShader fragment_shader;

        public ProgramObject(VertexShader vertex_shader, FragmentShader fragment_shader)
        {
            this.vertex_shader = vertex_shader;
            this.fragment_shader = fragment_shader;

            if (!vertex_shader.compiled)
                vertex_shader.compileShader();

            if (!fragment_shader.compiled)
                fragment_shader.compileShader();
            
            program_handle = GL.CreateProgram();

            GL.AttachShader(program_handle, vertex_shader.shader_handle);
            GL.AttachShader(program_handle, fragment_shader.shader_handle);

            GL.LinkProgram(program_handle);
        }
    }
}
