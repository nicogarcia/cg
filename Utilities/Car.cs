using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities.Shaders;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Utilities
{
	public class Car : WavefrontObj
	{
		public Vector4 position = new Vector4();
		public Vector4 heading = new Vector4(1f, 0, 0, 1f);

		public List<CarCamera> car_cameras = new List<CarCamera>();

		public Car(string path, ProgramObject program, Material material = null) :
			base(path, program, material)
		{
			transformation = Matrix4.CreateRotationZ((float)Math.PI) * transformation;
		}

		public void rotate(float angle)
		{
			heading = Vector4.Transform(heading, Matrix4.CreateRotationZ(angle));
			heading.Normalize();
			transformation = Matrix4.CreateRotationZ(angle) * transformation;

			notifyCameras();
		}

		public void move(float d_pos)
		{
			Matrix4 translation = Matrix4.CreateTranslation((d_pos * heading).Xyz);
			position += new Vector4((d_pos * heading).Xyz);
			transformation *= translation;
			notifyCameras();
		}

		public void notifyCameras()
		{
			foreach (CarCamera camera in car_cameras)
				camera.refresh();
		}

	}
}
