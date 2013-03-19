using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace CG_TP1
{
    // Contains the info necessary for each Drawable object
    class DrawingInfo
    {
        public int VAO_ID;
        public int VBO_ID;
        public BeginMode begin_mode;
    }

    // Handles the communication between the Drawable objects and OpenGl
    class GLManager
    {
        static Dictionary<Drawable, DrawingInfo> objects = new Dictionary<Drawable, DrawingInfo>();

        static bool shaders_set_up = false;
        static int vertex_shader_handle, fragment_shader_handle, program_shader_handle;

        const string DEFAULT_VERTEX_SHADER = @"
            #version 140
            in vec4 posicion;
            void main()
            { 
                gl_Position = posicion;
            }";
        const string DEFAULT_FRAGMENT_SHADER = @"
            #version 140
            out vec4 colorSalida;
            void main()
            {
                colorSalida = vec4(0.0f, 0.0f, 1f, 1f);
            }";

        static void setUpShaders()
        {
            vertex_shader_handle = GL.CreateShader(ShaderType.VertexShader);
            fragment_shader_handle = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(vertex_shader_handle, DEFAULT_VERTEX_SHADER);
            GL.ShaderSource(fragment_shader_handle, DEFAULT_FRAGMENT_SHADER);

            string info;

            GL.CompileShader(vertex_shader_handle);
            GL.GetShaderInfoLog(vertex_shader_handle, out info);
            Console.WriteLine("Compiling vertex shader... {0}", info);

            GL.CompileShader(fragment_shader_handle);
            GL.GetShaderInfoLog(fragment_shader_handle, out info);
            Console.WriteLine("Compiling fragmente shader... {0}", info);

            program_shader_handle = GL.CreateProgram();

            GL.AttachShader(program_shader_handle, vertex_shader_handle);
            GL.AttachShader(program_shader_handle, fragment_shader_handle);

            GL.LinkProgram(program_shader_handle);

            shaders_set_up = true;
        }

        public static void addDrawable(Drawable drawable, BeginMode begin_mode)
        {
            if (!shaders_set_up)
                setUpShaders();

            DrawingInfo drawing_info = new DrawingInfo();
            drawing_info.begin_mode = begin_mode;
            
            GL.GenVertexArrays(1, out drawing_info.VAO_ID);
            GL.GenBuffers(1, out drawing_info.VBO_ID);

            objects.Add(drawable, drawing_info);
        }

        public static void paintDrawable(Drawable drawable)
        {
            GL.UseProgram(program_shader_handle);

            DrawingInfo drawing_info = objects[drawable];

            GL.BindVertexArray(drawing_info.VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, drawing_info.VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(drawable.vertices.Length * Vector4.SizeInBytes),
                            drawable.vertices, BufferUsageHint.StaticDraw);
            
            GL.EnableVertexAttribArray(0);

            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.DrawArrays(drawing_info.begin_mode, 0, drawable.vertices.Length);

            GL.DisableVertexAttribArray(0);

            GL.UseProgram(0);

        }
    }
}
