using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;
using OpenTK;
using Tao.FreeGlut;

namespace Utilities
{
    public class GlutDraw : Drawable3D
    {
		public Action glutPrimitive;

		public GlutDraw(ProgramObject program, Action primitive)
            : base(program)
        {
			this.glutPrimitive = primitive;
        }

        public override void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix)
        {
            GL.UseProgram(program.program_handle);
			
            #region Tranformation Uniforms
            modelViewMatrix = transformation * modelViewMatrix;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref modelViewMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);
            #endregion

			glutPrimitive.Invoke();

            GL.UseProgram(0);
        }


    }
}
