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

        public Vector4 position = new Vector4();

        public WavefrontObj(string path, ProgramObject program, BeginMode begin_mode)
            : base(program, begin_mode)
        {
            SharpObjLoader obj = new SharpObjLoader(path);

            toDraw = obj.vertices.ToArray();

            base.fillArrayBuffer(false);
        }

        public override void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix)
        {

            GL.UseProgram(program.program_handle);

            modelViewMatrix = transformation * modelViewMatrix;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref modelViewMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);

            GL.Uniform4(light_position_location, 0.0f, 5.0f, 10.0f, 1f);

            // Light Intensity?
            GL.Uniform3(light_intensity_location, 0.5f, 0.5f, 0.5f);

            // Silver ADSs
            // 0.19225	0.19225	0.19225	0.50754	0.50754	0.50754	0.508273	0.508273	0.508273	0.4
            GL.Uniform3(material_ka_location, 1f, 1f, 1f);
            GL.Uniform3(material_kd_location, 0.02f, 0.02f, 0.05f);
            GL.Uniform3(material_ks_location, 0.5f, 0.5f, 0.5f);
            GL.Uniform1(material_shine_location, 2f);
            GL.Uniform1(colored_location, colored ? 1.0f : 0f);

            GL.BindVertexArray(VAO_ID);
            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawElements(BeginMode.Triangles, ebo_array.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

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
