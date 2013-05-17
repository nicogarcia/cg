using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utilities.Shaders
{
    public class Shaders
    {
        #region V_DEFAULT
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
        #endregion

        #region V_TRANSF
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
        #endregion

        #region V_LATEST
        public const string VERTEX_SHADER_LATEST = @"
            #version 330
            //layout (location = 0) in vec4 position;
            //layout (location = 1) in vec4 color;
            //layout (location = 2) in vec4 normal;
            in vec4 position;
            in vec4 normal;
            in vec4 color;
            in vec4 texture;

            out vec4 Position; 
            out vec4 Normal;  
            out vec4 Color;
            out vec2 TexCoord;

            uniform mat4 projectionMatrix;
            uniform mat4 modelView;

            void main(){
                Position = position;
                Normal = normal;
                TexCoord = vec2(texture);

                gl_Position = projectionMatrix * modelView * position;

                float rojo = 1.0 - abs(position.x);
                float verde = 1.0 - abs(position.y);
                float azul = abs(position.x + position.y);

                Color = vec4(1.0, 0, 0, 1.0);
            } 
            "; 
        #endregion

        #region V_TEXTURE
        public const string VERTEX_SHADER_TEXTURE = @"
            #version 330
            //layout (location = 0) in vec3 VertexPosition; 
            //layout (location = 1) in vec3 VertexNormal; 
            //layout (location = 2) in vec2 VertexTexCoord;
            in vec4 VertexPosition; 
            in vec4 VertexNormal;
            in vec4 VertexColor; 
            in vec4 VertexTexCoord; 
                
            out vec4 Position;
            out vec4 Normal;
            out vec4 Color;
            out vec2 TexCoord; 
                
            uniform mat4 modelView;
            uniform mat4 projectionMatrix;
            uniform mat4 normalMatrix;
            //uniform mat4 MVP; WHAT'S THIS?
                
            void main()
            {
                TexCoord = vec2(VertexTexCoord);
                Normal = normalize( normalMatrix * VertexNormal);
                Position = modelView * VertexPosition;
                //gl_Position = MVP * VertexPosition;
                gl_Position = projectionMatrix * modelView * VertexPosition;
            }
        ";
        #endregion

        #region F_DEFAULT
        public const string DEFAULT_FRAGMENT_SHADER = @"
            #version 330
            out vec4 fragment_output_color;
            in vec4 fragment_input_color;
            void main()
            {
                fragment_output_color = fragment_input_color;
            }";
        #endregion

        #region F_TEXTURE
        public const string FRAGMENT_SHADER_TEXTURE = @"
            #version 330
            in vec4 Position; 
            in vec4 Normal;
            in vec4 Color;
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
            //layout( location = 0 ) out vec4 FragColor;
            out vec4 FragColor;

            void phongModel( vec4 pos, vec4 norm, out vec3 ambAndDiff, out vec3 spec ) { 
                // Compute the ADS shading model here, return ambient 
                // and diffuse color in ambAndDiff, and return specular 
                // color in spec
            }
            void main() { 
                vec3 ambAndDiff, spec; 
                vec4 texColor = texture( Tex1, TexCoord ); 
                phongModel(Position, Normal, ambAndDiff, spec); 
                //FragColor = vec4(ambAndDiff, 1.0) * texColor + vec4(spec, 1.0);
                FragColor = texColor;
            }
        ";
        #endregion

        #region F_ILLUMINATION
        public const string FRAGMENT_SHADER_ILLUMINATION = @"
            #version 330
            in vec4 Position;
            in vec4 Normal;
            in vec4 Color;
            in vec2 TexCoord;
            uniform sampler2D Tex1;

            uniform vec4 light_position;
            uniform vec3 light_intensity;
            uniform vec3 material_ka;
            uniform vec3 material_kd;
            uniform vec3 material_ks;
            uniform float material_shine;

            //layout( location = 0 ) out vec4 FragColor;
            out vec4 FragColor;

            void blinnPhongModel( vec4 L, vec4 N, vec4 H, out vec3 ambAndDiff, out vec3 spec ) {
                // Compute the ADS shading model here, return ambient
                // and diffuse color in ambAndDiff, and return specular
                // color in spec
                float diffuse = max( dot(L, N), 0.0);
                ambAndDiff = material_ka + material_kd * diffuse;
                spec = dot(L, N) < 0.0 ? vec3(0,0,0) : pow(max(dot(N, H), 0.0), material_shine);
            }

            void main() {
                vec4 V = normalize(Position);
                vec4 L= normalize(light_position);
                vec4 N = normalize(Normal);            
                vec4 H = normalize(L+V);

                vec3 ambAndDiff, spec;
                vec4 texColor = texture( Tex1, TexCoord );//vec4(0.2, 0.2,0.2,1.0)
                blinnPhongModel(L, N, H, ambAndDiff, spec);

                FragColor = vec4(ambAndDiff, 1.0) * texColor + vec4(spec, 1.0);
                //FragColor = vec4(texColor);
            }
        ";
        #endregion
    }
}
