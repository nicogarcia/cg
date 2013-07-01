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
        public int VAO_ID, VBO_ID, EBO_ID, NVBO_ID, NEBO_ID, NVAO_ID;

        public Matrix4 transformation = Matrix4.Identity;

        public ProgramObject program;
        public bool colored = true;
		public bool draw_normals = false;
		public bool noise_texture = false;
		public int illumination_model = 0;

		public Light light = new Light(new Spherical(5, MathHelper.PiOver2, 0));

        protected int projection_location, model_view_location, normal_location, light_position_location;
        protected int light_intensity_location, material_ka_location, material_kd_location;
		protected int material_ks_location, material_shine_location, colored_location, alpha_location, noise_location;
		protected int illumination_model_location;
		protected int roughness_location, reflect_location;

        public int[] indices, count;
        public int[] ebo_array;
        public int[] normals_ebo_array;

        public Vertex[] toDraw;
        public List<int> ebo_list = new List<int>();
        public List<EBO> ebos = new List<EBO>();

        public Drawable3D(ProgramObject program)
        {
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
            alpha_location = GL.GetUniformLocation(program.program_handle, "alpha");
			noise_location = GL.GetUniformLocation(program.program_handle, "noise");
			illumination_model_location = GL.GetUniformLocation(program.program_handle, "illum_model");
			roughness_location = GL.GetUniformLocation(program.program_handle, "fRoughness");
			reflect_location = GL.GetUniformLocation(program.program_handle, "reflecAtNormalIncidence");

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
            #region VBO
            Vector4[] vertexArray = VertexArray.singleVector4Array(toDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertexArray.Length * Vector4.SizeInBytes),
                vertexArray, BufferUsageHint.StaticDraw);
            #endregion

            #region EBO
            // Transform indices from indices[] and count[] arrays
            if (transform_indices)
            {
                ebo_array = ebo_from_indices();

                GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO_ID);
                GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ebo_array.Length * sizeof(int)),
                    ebo_array, BufferUsageHint.StaticDraw);
            }
            else
            {
                foreach (EBO ebo in ebos)
                {
                    int id;
                    GL.GenBuffers(1, out id);
                    ebo.id = id;

                    ebo.indices = new int[ebo.lastIndex - ebo.firstIndex + 1];
                    for (int i = 0; i < ebo.indices.Length; i++)
                        ebo.indices[i] = i + ebo.firstIndex;

                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo.id);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(ebo.indices.Length * sizeof(int)),
                        ebo.indices, BufferUsageHint.StaticDraw);
                }
            }

            #endregion

            set_vao(vertexArray);

			set_nvao(vertexArray);
        }

        private void set_nvao(Vector4[] vertexArray)
        {
            #region NBO
            Vector4[] normalsArray = new Vector4[toDraw.Length * 2];
            for (int i = 0; i < toDraw.Length; i++)
            {
                normalsArray[2 * i] = new Vector4(vertexArray[i]);
                Vector4 normalized;
                Vector4.Normalize(ref vertexArray[i + toDraw.Length], out normalized);
                Vector4.Add(ref normalized, ref vertexArray[i], out normalsArray[2 * i + 1]);
            }

            GL.BindBuffer(BufferTarget.ArrayBuffer, NVBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(normalsArray.Length * Vector4.SizeInBytes),
                normalsArray, BufferUsageHint.StaticDraw);
            #endregion

            #region NEBO
            normals_ebo_array = new int[toDraw.Length * 2];

            for (int i = 0; i < normalsArray.Length; i++)
            {
                normals_ebo_array[i] = i;
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, NEBO_ID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(normals_ebo_array.Length * sizeof(int)),
                normals_ebo_array, BufferUsageHint.StaticDraw);
            #endregion

            #region NVAO
            GL.BindVertexArray(NVAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, NVBO_ID);

            // Position attrib
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
            GL.BindAttribLocation(program.program_handle, 0, "VertexPosition");


            GL.BindBuffer(BufferTarget.ElementArrayBuffer, NEBO_ID);

            GL.BindVertexArray(0);
            #endregion
        }

        private void set_vao(Vector4[] vertexArray)
        {
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
        }

        private int[] ebo_from_indices()
        {
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