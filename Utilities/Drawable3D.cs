using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;

namespace Utilities
{
    public abstract class Drawable3D
    {
        public int VAO_ID;
        public int VBO_ID;

        public TriangleCollection triangles;
        public Matrix4 transformation = Matrix4.Identity;
        public ProgramObject program;
        public BeginMode begin_mode;

        private int projection_location;
        private int model_view_location;

        private Vector4[] toDraw;

        public Drawable3D(ProgramObject program, BeginMode begin_mode)
        {
            triangles = new TriangleCollection();
            this.begin_mode = begin_mode;
            this.program = program;

            projection_location = GL.GetUniformLocation(program.program_handle, "projectionMatrix");
            model_view_location = GL.GetUniformLocation(program.program_handle, "modelView");

            GL.GenVertexArrays(1, out VAO_ID);
            GL.GenBuffers(1, out VBO_ID);
        }

        public void reloadTriangles()
        {
            toDraw = triangles.getArray();
            int cant = toDraw.Length / 3;

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

            // Color attrib
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, cant * Vector4.SizeInBytes);

            // Normal attrib
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 2 * cant * Vector4.SizeInBytes);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(toDraw.Length * Vector4.SizeInBytes),
                            toDraw, BufferUsageHint.StaticDraw);
        }

        public void paint(Matrix4 projMatrix, Matrix4 zoomMatrix)
        {
            GL.UseProgram(program.program_handle);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);

            GL.DrawArrays(begin_mode, 0, toDraw.Length / 3);

            GL.UseProgram(0);
        }
    }
}
