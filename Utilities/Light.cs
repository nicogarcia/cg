using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
	public class Light
	{
		public Spherical position;
		
		public Light(Spherical pos)
		{
			this.position = pos;
		}

		public void moveBack()
		{
			position.moveBack();
		}

		public void moveForward()
		{
			position.moveForward();
		}

		public void growTheta()
		{
			position.growTheta();
		}

		public void growPhi()
		{
			position.growPhi();
		}

		public void shrinkPhi()
		{
			position.shrinkPhi();
		}

		public void shrinkTheta()
		{
			position.shrinkTheta();
		}
		
	}
}
