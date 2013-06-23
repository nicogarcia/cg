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
        public bool draw_normals = false;

        protected int projection_location, model_view_location, normal_location, light_position_location;
        protected int light_intensity_location, material_ka_location, material_kd_location;
        protected int material_ks_location, material_shine_location, colored_location;



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

            // Vertex Data
            GL.GenBuffers(1, out VBO_ID);
            GL.GenBuffers(1, out EBO_ID);
            GL.GenVertexArrays(1, out VAO_ID);

            // Normal Data
            GL.GenBuffers(1, out NVBO_ID);
            GL.GenBuffers(1, out NEBO_ID);
            GL.GenVertexArrays(1, out NVAO_ID);
        }

        public virtual void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix) { }

        public void fillArrayBuffer(bool transform_indices)
        {

            /**** VBO ****/
            Vector4[] vertexArray = VertexArray.singleVector4Array(toDraw);
            
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Vector4.SizeInBytes),
                vertexArray, BufferUsageHint.StaticDraw);
            /**************/

            /**** EBO ****/
            if (transform_indices)
                ebo_array = ebo_from_indices();
            else{
                ebo_array = new int[toDraw.Length];
                for (int i = 0; i < ebo_array.Length; i++)
                    ebo_array[i] = i;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ebo_array.Length * sizeof(int)),
                ebo_array, BufferUsageHint.StaticDraw);
            
            /*************/

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

            if (draw_normals)
            {

                /**** NBO ****/

                Vector4[] normalsArray = new Vector4[toDraw.Length * 2];
                for (int i = 0; i < toDraw.Length; i++)
                {
                    normalsArray[2 * i] = new Vector4(vertexArray[i]);
                    Vector4 normalized;
                    Vector4.Normalize(ref vertexArray[i + toDraw.Length], out normalized);
                    //Vector4.Multiply(normalized, -1.0f);
                    Vector4.Add(ref normalized, ref vertexArray[i], out normalsArray[2 * i + 1]);
                }

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

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, NEBO_ID);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(normals_ebo_array.Length * sizeof(int)),
                    normals_ebo_array, BufferUsageHint.StaticDraw);

                /*************/

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

        private int[] ebo_from_indices() {
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

            return ebo_list.ToArray();
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