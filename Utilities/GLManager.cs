using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using CG_TP1.Shapes;

namespace CG_TP1
{
    // Contains the info necessary for each Drawable object
    class DrawingInfo
    {
        public int VAO_ID;
        public int VBO_ID;
        public ProgramObject program;
        public BeginMode begin_mode;
    }

    // Handles the communication between the Drawable objects and OpenGl
    public class GLManager
    {
        static Dictionary<Drawable, DrawingInfo> objects = new Dictionary<Drawable, DrawingInfo>();

        public static void addDrawable(Drawable drawable, BeginMode begin_mode, ProgramObject program)
        {
            DrawingInfo drawing_info = new DrawingInfo();
            drawing_info.begin_mode = begin_mode;
            drawing_info.program = program;
            
            GL.GenVertexArrays(1, out drawing_info.VAO_ID);
            GL.GenBuffers(1, out drawing_info.VBO_ID);

            objects.Add(drawable, drawing_info);
        }

        public static void paintDrawable(Drawable drawable)
        {
            DrawingInfo drawing_info = objects[drawable];

            GL.UseProgram(drawing_info.program.program_handle);

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

        public static void refreshViewport(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

    }
}
