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

                fragment_input_color = vec4(0, 0, 0, 1.0);
            } 
            ";

        public const string VERTEX_SHADER_LATEST = @"
            #version 330
            out vec4 fragment_input_color;
            //layout (location = 0) in vec4 position;
            //layout (location = 1) in vec4 color;
            //layout (location = 2) in vec4 normal;
            in vec4 position;
            in vec4 color;
            in vec4 normal;

            uniform mat4 projectionMatrix;
            uniform mat4 modelView;

            void main(){
                gl_Position = projectionMatrix * modelView * position;

                float rojo = 1.0 - abs(position.x);
                float verde = 1.0 - abs(position.y);
                float azul = abs(position.x + position.y);

                fragment_input_color = vec4(rojo, verde, azul, 1.0);
            } 
            ";

        public const string VERTEX_SHADER_TEXTURE = @"
                //layout (location = 0) in vec3 VertexPosition; 
                //layout (location = 1) in vec3 VertexNormal; 
                //layout (location = 2) in vec2 VertexTexCoord;
                in vec4 VertexPosition; 
                in vec4 VertexNormal; 
                in vec2 VertexTexCoord; 
                
                out vec4 Position; 
                out vec4 Normal; 
                out vec2 TexCoord; 
                
                uniform mat4 ModelViewMatrix; 
                uniform mat4 NormalMatrix; 
                uniform mat4 ProjectionMatrix; 
                uniform mat4 MVP;
                
                void main() 
                { 
                    TexCoord = VertexTexCoord; 
                    Normal = normalize( NormalMatrix * VertexNormal); 
                    Position = ModelViewMatrix * VertexPosition; 
                    gl_Position = MVP * VertexPosition; 
                }
        ";

        public const string FRAGMENT_SHADER_TEXTURE = @"
            in vec4 Position; 
            in vec4 Normal; 
            in vec2 TexCoord;
            uniform sampler2D Tex1;
            struct LightInfo { 
                vec4 Position; // Light position in eye coords. 
                vec3 Intensity; // A,D,S intensity
            }; 
            uniform LightInfo Light;
            struct MaterialInfo { 
                vec3 Ka; // Ambient reflectivity
                vec3 Kd; // Diffuse reflectivity
                vec3 Ks; // Specular reflectivity
                float Shininess; // Specular shininess factor 
            }; 
            uniform MaterialInfo Material; 
            layout( location = 0 ) out vec4 FragColor;

            void phongModel( vec4 pos, vec4 norm, out vec3 ambAndDiff, out vec3 spec ) { 
                // Compute the ADS shading model here, return ambient 
                // and diffuse color in ambAndDiff, and return specular 
                // color in spec
            }
            void main() { 
                vec3 ambAndDiff, spec; 
                vec4 texColor = texture( Tex1, TexCoord ); 
                phongModel(Position, Normal, ambAndDiff, spec); 
                FragColor = vec4(ambAndDiff, 1.0) * texColor + vec4(spec, 1.0); 
            }
        ";
    }
}
