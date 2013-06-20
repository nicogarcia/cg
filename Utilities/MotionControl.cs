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
        static float k_accel = 0.08f;
        // Deacceleration constant
        static float k_deaccel = -0.05f;
        // Friction constant
        static float k_friction = -k_accel / 10;
        // Handling constant (speed incidence over turn angle)
        static float k_handling = 0.006f;

        static float speed = 0;

        static bool[] pressed_keys = new bool[256];

        public static void refreshCamera(GhostCamera camera)
        {
            bool accelerating = pressed_keys[(int)Keys.W];
            bool deaccelerating = pressed_keys[(int)Keys.S];
            bool turning_right = pressed_keys[(int)Keys.D];
            bool turning_left = pressed_keys[(int)Keys.A];

            bool turning = turning_left | turning_right;
            
            float accel = 0, angle = 0;

            // Accelerating
            if (accelerating)
                accel = k_accel;

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

            camera.move(speed);
            camera.rotate(angle);
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
