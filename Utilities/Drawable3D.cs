using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;
using Utilities;

namespace Utilities
{
    public abstract class Drawable3D
    {
        public int VAO_ID;
        public int VBO_ID;

        public Matrix4 transformation = Matrix4.Identity;
        public ProgramObject program;
        public BeginMode begin_mode;

        protected int projection_location;
        protected int model_view_location;
        protected int normal_location;
        protected int light_position_location;
        protected int light_intensity_location;
        protected int material_ka_location;
        protected int material_kd_location;
        protected int material_ks_location;
        protected int material_shine_location;

        public Vertex[] toDraw;

        public Drawable3D(ProgramObject program, BeginMode begin_mode)
        {
            this.begin_mode = begin_mode;
            this.program = program;

            projection_location = GL.GetUniformLocation(program.program_handle, "projectionMatrix");
            model_view_location = GL.GetUniformLocation(program.program_handle, "modelView");
            normal_location = GL.GetUniformLocation(program.program_handle, "normalMatrix");

            light_position_location = GL.GetUniformLocation(program.program_handle, "light_position");
            light_intensity_location = GL.GetUniformLocation(program.program_handle, "light_intensity");
            material_ka_location = GL.GetUniformLocation(program.program_handle, "material_ka");
            material_kd_location = GL.GetUniformLocation(program.program_handle, "material_kd");
            material_ks_location = GL.GetUniformLocation(program.program_handle, "material_ks");
            material_shine_location = GL.GetUniformLocation(program.program_handle, "material_shine");

            GL.GenVertexArrays(1, out VAO_ID);
            GL.GenBuffers(1, out VBO_ID);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);

            // Color attrib
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);

            // Normal attrib
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);

            // Texture attrib
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 4 * Vector4.SizeInBytes, 0);

        }

        public virtual void paint(Matrix4 projMatrix, Matrix4 zoomMatrix)
        {
            GL.UseProgram(program.program_handle);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            projMatrix = projMatrix * transformation;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(projMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);

            GL.DrawArrays(begin_mode, 0, toDraw.Length / 3);

            GL.UseProgram(0);
        }

        public void fillArrayBuffer()
        {
            // Load BufferData with "draw" array data
            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(toDraw.Length * 4 * Vector4.SizeInBytes),
                            VertexArray.singleVector4Array(toDraw), BufferUsageHint.StaticDraw);
        }
    }
}
