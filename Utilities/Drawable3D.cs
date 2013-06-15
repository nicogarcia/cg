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
        public int EBO_ID;

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
        public int[] indices, count;
        public int[] ebo_array;
        public List<int> ebo_list = new List<int>();

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

        }

        public virtual void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix)
        {
            /* GL.UseProgram(program.program_handle);

             GL.BindVertexArray(VAO_ID);
             GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

             modelViewMatrix *= transformation;
             Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(projMatrix));

             GL.UniformMatrix4(projection_location, false, ref projMatrix);
             GL.UniformMatrix4(model_view_location, false, ref modelViewMatrix);
             GL.UniformMatrix4(normal_location, false, ref normalMatrix);

             GL.DrawArrays(begin_mode, 0, toDraw.Length / 3);

             GL.UseProgram(0);*/
        }

        public void fillArrayBuffer()
        {
            Vector4[] vertexArray = VertexArray.singleVector4Array(toDraw);

            /* Vertex array of cube */

            /*vertexArray = new Vector4[]{
                // Position
                new Vector4(1.0f, 1.0f, 10.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                // Normal
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                // Color
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(-10.0f, 10.0f, 1.0f, 1.0f),
                new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
                //Tex
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
                new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
            };*/

            GL.GenBuffers(1, out VBO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Vector4.SizeInBytes),
                vertexArray, BufferUsageHint.StaticDraw);

            /*ebo_list = new List<int>();

            for (int j = 0; j < indices.Length; j++)
            {
                for (int i = 0; i < count[j] - 2; i++)
                {
                    ebo_list.Add(indices[j] + i);
                    ebo_list.Add(indices[j] + i + 1);
                    ebo_list.Add(indices[j] + i + 2);
                }
            }*/

            ebo_array = ebo_list.ToArray();
            /*
            ebo_array = new int[]{
                0,1,2
            };*/

            GL.GenBuffers(1, out EBO_ID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ebo_array.Length * sizeof(int)),
                ebo_array, BufferUsageHint.StaticDraw);

            GL.GenVertexArrays(1, out VAO_ID);
            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

            // Normal attrib
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, vertexArray.Length / 4 * Vector4.SizeInBytes);

            // Color attrib
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 2 * vertexArray.Length / 4 * Vector4.SizeInBytes);

            // Texture attrib
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 0, 3 * vertexArray.Length / 4 * Vector4.SizeInBytes);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);

            GL.BindVertexArray(0);



            /*// Load BufferData with "draw" array data

            GL.GenBuffers(1, out VBO_ID);
            GL.GenVertexArrays(1, out VAO_ID);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            Vector4[] vertexArray = VertexArray.singleVector4Array(toDraw);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vertexArray.Length * Vector4.SizeInBytes),
                            vertexArray, BufferUsageHint.StaticDraw);


            // Position attrib
            GL.EnableVertexAttribArray(5);
            GL.BindAttribLocation(program.program_handle, 5, "VertexPosition");
            GL.VertexAttribPointer(5, 4, VertexAttribPointerType.Float, false, 0, 0);

            // Normal attrib
            GL.EnableVertexAttribArray(10);
            GL.BindAttribLocation(program.program_handle, 10, "VertexNormal");
            GL.VertexAttribPointer(10, 4, VertexAttribPointerType.Float, false, 0, vertexArray.Length / 4 * Vector4.SizeInBytes);

            // Color attrib
            GL.EnableVertexAttribArray(2);
            GL.BindAttribLocation(program.program_handle, 2, "VertexColor");
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 2 * vertexArray.Length / 4 * Vector4.SizeInBytes);

            // Texture attrib
            GL.EnableVertexAttribArray(3);
            GL.BindAttribLocation(program.program_handle, 3, "VertexTexCoord");
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 0, 3 * vertexArray.Length / 4 * Vector4.SizeInBytes);
            */
        }
    }
}
