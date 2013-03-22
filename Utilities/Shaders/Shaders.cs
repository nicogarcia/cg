using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CG_TP1
{
    public class Shaders
    {
        public const string DEFAULT_VERTEX_SHADER = @"
            #version 140
            out vec4 fragment_input_color;
            in vec4 position;
            void main()
            { 
                gl_Position = position;
                float red = 1.0-abs(position.x);
                float green = 1.0-abs(position.y);
                float blue = abs(position.x + position.y);
                fragment_input_color = vec4(red, green, blue, 1.0f);
            }";
        public const string DEFAULT_FRAGMENT_SHADER = @"
            #version 140
            out vec4 fragment_output_color;
            in vec4 fragment_input_color;
            void main()
            {
                fragment_output_color = fragment_input_color;
            }";

    }
}
