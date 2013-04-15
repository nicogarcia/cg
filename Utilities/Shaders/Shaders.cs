using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Shaders
{
    public class Shaders
    {
        public const string DEFAULT_VERTEX_SHADER = @"
            #version 330
            out vec4 fragment_input_color;
            //layout (location = 0) in vec4 position;
            //layout (location = 1) in vec4 input_color;
            in vec4 position;
            in vec4 input_color;
            void main()
            { 
                gl_Position = position;

                fragment_input_color = input_color;
            }";
        public const string DEFAULT_FRAGMENT_SHADER = @"
            #version 330
            out vec4 fragment_output_color;
            in vec4 fragment_input_color;
            void main()
            {
                fragment_output_color = fragment_input_color;
            }";

        public const string VERTEX_TRANSF_SHADER = @"
            #version 330
            out vec4 fragment_input_color;
            //layout (location = 0) in vec4 position;
            //layout (location = 1) in vec4 input_color;
            in vec4 position;
            in vec4 input_color;
            uniform mat4 projectionMatrix;
            uniform mat4 modelView;
            void main(){
                gl_Position = projectionMatrix * modelView * position;

                float rojo = 1.0 - abs(position.x);
                float verde = 1.0 - abs(position.y);
                float azul = abs(position.x + position.y);

                fragment_input_color = vec4(rojo, azul, verde, 1.0);
            } 
            ";
    }
}
