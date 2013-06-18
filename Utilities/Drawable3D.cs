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
        public int NVBO_ID;
        public int NEBO_ID;
        public int NVAO_ID;

        public Matrix4 transformation = Matrix4.Identity;
        public ProgramObject program;
        public BeginMode begin_mode;
        public bool colored = true;

        protected int projection_location;
        protected int model_view_location;
        protected int normal_location;
        protected int light_position_location;
        protected int light_intensity_location;
        protected int material_ka_location;
        protected int material_kd_location;
        protected int material_ks_location;
        protected int material_shine_location;
        protected int colored_location;

        public Vertex[] toDraw;
        public int[] indices, count;
        public int[] ebo_array;
        public int[] normals_ebo_array;
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
            colored_location = GL.GetUniformLocation(program.program_handle, "colored");

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

            /**** VBO ****/
            Vector4[] vertexArray = VertexArray.singleVector4Array(toDraw);
            
            GL.GenBuffers(1, out VBO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Vector4.SizeInBytes),
                vertexArray, BufferUsageHint.StaticDraw);
            /**************/

            /**** EBO ****/
            ebo_list = new List<int>();

            for (int j = 0; j < indices.Length; j++)
            {
                for (int i = 0; i < count[j] - 2; i++)
                {
                    ebo_list.Add(indices[j] + i);
                    ebo_list.Add(indices[j] + i + 1);
                    ebo_list.Add(indices[j] + i + 2);
                }
            }

            ebo_array = ebo_list.ToArray();
            GL.GenBuffers(1, out EBO_ID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ebo_array.Length * sizeof(int)),
                ebo_array, BufferUsageHint.StaticDraw);
            
            /*************/

            GL.GenVertexArrays(1, out VAO_ID);
            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindAttribLocation(program.program_handle, 0, "VertexPosition");

            // Normal attrib
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, 0, vertexArray.Length / 4 * Vector4.SizeInBytes);
            GL.BindAttribLocation(program.program_handle, 1, "VertexNormal");

            // Color attrib
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 4, VertexAttribPointerType.Float, false, 0, 2 * vertexArray.Length / 4 * Vector4.SizeInBytes);
            GL.BindAttribLocation(program.program_handle, 2, "VertexColor");

            // Texture attrib
            GL.EnableVertexAttribArray(3);
            GL.VertexAttribPointer(3, 4, VertexAttribPointerType.Float, false, 0, 3 * vertexArray.Length / 4 * Vector4.SizeInBytes);
            GL.BindAttribLocation(program.program_handle, 3, "VertexTexCoord");

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);

            GL.BindVertexArray(0);



            /**** NBO ****/

            Vector4[] normalsArray = new Vector4[toDraw.Length * 2];
            for (int i = 0; i < toDraw.Length; i++)
            {
                normalsArray[2 * i] = new Vector4(vertexArray[i]);
                Vector4 normalized;
                Vector4.Normalize(ref vertexArray[i + toDraw.Length], out normalized);
                Vector4.Multiply(normalized, -1.0f);
                //normalsArray[2 * i + 1] = vertexArray[i + toDraw.Length] + vertexArray[i];
                Vector4.Add(ref normalized, ref vertexArray[i], out normalsArray[2 * i + 1]);
                //Vector4.Transform(normalsArray[2 * i + 1],Matrix4.CreateRotationX(0.2f) * Matrix4.CreateRotationY(0.2f));
            }

            GL.GenBuffers(1, out NVBO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NVBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalsArray.Length * Vector4.SizeInBytes),
                normalsArray, BufferUsageHint.StaticDraw);

            /**************/
            /**** NEBO ****/
            normals_ebo_array = new int[toDraw.Length * 2];

            for (int i = 0; i < normalsArray.Length; i++)
            {
                normals_ebo_array[i] = i;
            }

            GL.GenBuffers(1, out NEBO_ID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, NEBO_ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(normals_ebo_array.Length * sizeof(int)),
                normals_ebo_array, BufferUsageHint.StaticDraw);

            /*************/

            GL.GenVertexArrays(1, out NVAO_ID);
            GL.BindVertexArray(NVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NVBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindAttribLocation(program.program_handle, 0, "VertexPosition");


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, NEBO_ID);

            GL.BindVertexArray(0);
        }
    }
}
/* Vertex array of cube */

/*vertexArray = new Vector4[]{
    // Position
    new Vector4(1.0f, 1.0f, 1.0f, 1.0f),
    new Vector4(-1.0f, 1.0f, 1.0f, 1.0f),
    new Vector4(-1.0f, -1.0f, 1.0f, 1.0f),
    // Normal
    new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
    new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
    new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
    // Color
    new Vector4(1.0f, 0.0f, 0.0f, 1.0f),
    new Vector4(0.0f, 1.0f, 0.0f, 1.0f),
    new Vector4(0.0f, 0.0f, 1.0f, 1.0f),
    //Tex
    new Vector4(0.5f, 0.5f, 1.0f, 1.0f),
    new Vector4(0.75f, 0.75f, 1.0f, 1.0f),
    new Vector4(0.0f, 0.25f, 1.0f, 1.0f),
};*/

/*ebo_array = new int[]{
    0,1,2
};*/