using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;
using OpenTK;

namespace Utilities
{
    public class WavefrontObj : Drawable3D
    {
		public Material material { get; set; }

        public WavefrontObj(string path, ProgramObject program, BeginMode begin_mode, Material material = null)
            : base(program, begin_mode)
        {
            this.material = material;
            SharpObjLoader obj = new SharpObjLoader(path, material);
            
            ebos = obj.ebos;

            toDraw = obj.vertices.ToArray();

			//draw_normals = true;

            base.fillArrayBuffer(false);
        }

        public override void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix)
        {
            GL.UseProgram(program.program_handle);

            #region Light Uniforms
            GL.Uniform4(light_position_location, 25.0f, 25.0f, 550.0f, 1f);
            // Light Intensity? Not used in shaders!
            GL.Uniform3(light_intensity_location, 0.5f, 0.5f, 0.5f);
            #endregion

            #region Tranformation Uniforms
            modelViewMatrix = transformation * modelViewMatrix;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref modelViewMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);
            #endregion

            #region Material Uniforms
            GL.Uniform3(material_ka_location, 1f, 1f, 1f);
            GL.Uniform3(material_kd_location, 0.02f, 0.02f, 0.05f);
            GL.Uniform3(material_ks_location, 0.5f, 0.5f, 0.5f);
            GL.Uniform1(material_shine_location, 2f);
            GL.Uniform1(colored_location, colored ? 1.0f : 0f);
            #endregion

            GL.BindVertexArray(VAO_ID);

            foreach (EBO ebo in ebos)
            {
                GL.Uniform3(material_ka_location, ebo.material.Ambient);
                GL.Uniform3(material_kd_location, ebo.material.Diffuse);
                GL.Uniform3(material_ks_location, ebo.material.Specular);
                GL.Uniform1(material_shine_location, ebo.material.Shininess);
				GL.Uniform1(alpha_location, ebo.material.Alpha);
				GL.Uniform1(colored_location, colored ? 1.0f : 0f);

                if (ebo.material.Texture != null)
                {
                    GL.Uniform1(colored_location, 0f);
                    Textures.LoadTexture(ebo.material.Texture.filename);
                }


                GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo.id);
                GL.DrawElements(BeginMode.Triangles, ebo.indices.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

                GL.BindTexture(TextureTarget.Texture2D, 0);
            }

            GL.BindVertexArray(0);

            if (draw_normals)
            {
                GL.BindVertexArray(NVAO_ID);

                GL.DrawElements(BeginMode.Lines, normals_ebo_array.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

                GL.BindVertexArray(0);
            }

            GL.UseProgram(0);
        }


    }
}
