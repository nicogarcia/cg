using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities
{
    public class Spherical
    {
        float radio, theta, phi;

        public float x, y, z;

        public Spherical(float radio, float theta, float phi)
        {
            this.radio = radio;
            this.theta = theta;
            this.phi = phi;

            setXYZ();
        }

        public Spherical(Spherical pos)
        {
            this.radio = pos.radio;
            this.theta = pos.theta;
            this.phi = pos.phi;

            setXYZ();
        }

        private void setXYZ()
        {
            x = (float)(radio * Math.Sin(theta) * Math.Cos(phi));
            y = (float)(radio * Math.Sin(theta) * Math.Sin(phi));
            z = (float)(radio * Math.Cos(theta));
        }

        public void moveBack()
        {
            radio += 0.5f;
            setXYZ();
        }

        public void growTheta()
        {
            theta += 0.1f;
            setXYZ();
        }

        public void growPhi()
        {
            phi += 0.1f;
            setXYZ();
        }

        public void moveForward()
        {
            if (radio <= 0.2f)
                return;
            radio -= 0.5f;
            setXYZ();
        }


        public void shrinkTheta()
        {
            theta -= 0.1f;
            setXYZ();
        }

        public void shrinkPhi()
        {
            phi -= 0.1f;
            setXYZ();
        }
    }
}
