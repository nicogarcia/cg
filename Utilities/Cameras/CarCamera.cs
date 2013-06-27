using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
	public class CarCamera : Camera
	{
		public Vector4 normal;
		public Vector4 position;
		public Vector4 distance;

		public Car car;

		public CarCamera(Car car)
		{
			this.normal = new Vector4(0, 0, 1, 1f);
			this.distance = new Vector4(2.2f, 0, 1.1f, 1);
			this.position = new Vector4(car.position + distance);
			this.car = car;
			car.car_cameras.Add(this);
		}

		public override Matrix4 lookAt()
		{
			return Matrix4.LookAt(position.Xyz, car.position.Xyz + new Vector3(5 * car.heading.X, 5  * car.heading.Y , 1), normal.Xyz);
		}

		public void refresh()
		{
			Vector2 scale = car.heading.Xy * distance.Xy.Length;
			position = car.position + new Vector4(
				new Vector3(scale.X, scale.Y, distance.Z));
		}
	}
}
