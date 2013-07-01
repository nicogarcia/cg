using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utilities
{
    public class MotionControl
    {
        // Acceleration constant
        static float k_accel = 0.05f;
        // Deacceleration constant
        static float k_deaccel = -0.2f;
        // Friction constant
        static float k_friction = -k_accel / 5;
        // Handling constant (speed incidence over turn angle)
        static float k_handling = 0.01f;

        public static float speed = 0;

        static PositionTracking position_tracking = new PositionTracking();

        static bool[] pressed_keys = new bool[256];

		public static Car car;

        public static void refreshCamera()
        {
            bool accelerating = pressed_keys[(int)Keys.W];
            bool deaccelerating = pressed_keys[(int)Keys.S];
            bool turning_right = pressed_keys[(int)Keys.D];
            bool turning_left = pressed_keys[(int)Keys.A];

            bool turning = turning_left | turning_right;
            
            float accel = 0, angle = 0;

            // Accelerating
            if (accelerating)
                // Check if it is going forward or braking
                if (speed >= 0)
                    accel = k_accel;
                else
                    accel = -k_deaccel;

            // Deaccelerating
            if (deaccelerating)
                // Check if it is going reverse or braking
                if (speed <= 0)
                    accel = -k_accel;
                else
                    accel = k_deaccel;

            // Friction acceleration term (always present)
            float friction_accel = Math.Sign(speed) * k_friction;
            accel += friction_accel;

            // If friction makes it stop, stop it
            if (Math.Sign(speed) != Math.Sign(speed + friction_accel))
                speed = 0;
            else
                speed += k_accel * (float) Math.Sqrt(Math.Abs(accel)) * (float) Math.Sign(accel);

            bool moving = speed != 0;

            if(moving && turning){
                int side = turning_left ? 1 : -1;
                int sign = speed > 0 ? side : -side;
                angle = k_handling / (float) Math.Sqrt(Math.Abs(speed)) * sign;
            }

			car.rotate(angle);
			car.move(speed);

            position_tracking.Teletransport(car.position);
        }

        public static void refreshCamera(SphericalCamera camera)
        {

            bool up = pressed_keys[(int)Keys.W];
            bool down = pressed_keys[(int)Keys.S];
            bool right = pressed_keys[(int)Keys.D];
            bool left = pressed_keys[(int)Keys.A];
			bool closer = pressed_keys[(int)Keys.Add];
			bool farther = pressed_keys[(int)Keys.Subtract];

            if (up)
                camera.position.growTheta();

            if (down)
                camera.position.shrinkTheta();

            if (right)
                camera.position.growPhi();

            if (left)
                camera.position.shrinkPhi();

			if (closer)
				camera.position.moveForward();

			if (farther)
				camera.position.moveBack();
            
        }

		public static void refreshLight(Light light)
		{

			bool up = pressed_keys[(int)Keys.I];
			bool down = pressed_keys[(int)Keys.K];
			bool right = pressed_keys[(int)Keys.L];
			bool left = pressed_keys[(int)Keys.J];

			if (up)
				light.position.growTheta();

			if (down)
				light.position.shrinkTheta();

			if (right)
				light.position.growPhi();

			if (left)
				light.position.shrinkPhi();

		}

        public static void keyDown(int key)
        {
            pressed_keys[key] = true;            
        }

        public static void keyUp(int key)
        {
            pressed_keys[key] = false;
        }

    }
}
