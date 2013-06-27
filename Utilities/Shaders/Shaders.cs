﻿using System;
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
            #version 140
            layout (location = 0) in vec4 VertexPosition; 
            layout (location = 1) in vec4 VertexNormal; 
            layout (location = 2) in vec4 VertexColor; 
            layout (location = 3) in vec4 VertexTexCoord;
            //in vec4 VertexPosition; 
            //in vec4 VertexNormal; 
            //in vec4 VertexColor; 
            //in vec4 VertexTexCoord;
                
            out vec4 Position;
            out vec4 Normal;
            out vec4 Color;
            out vec2 TexCoord; 
            out vec4 vLE;
                
            uniform vec4 light_position;

            uniform mat4 modelView;
            uniform mat4 projectionMatrix;
            uniform mat4 normalMatrix;
                
            void main()
            {
                TexCoord = vec2(VertexTexCoord);
                Normal = normalize( normalMatrix * VertexNormal);
                Position = modelView * VertexPosition;
                vLE = light_position - VertexPosition;
                Color = VertexColor;
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
            #version 140
            in vec4 Position;
            in vec4 Normal;
            in vec4 Color;
            in vec2 TexCoord;
            in vec4 vLE;            

            uniform sampler2D Tex1;

            uniform vec3 light_intensity;
            uniform vec3 material_ka;
            uniform vec3 material_kd;
            uniform vec3 material_ks;
            uniform float material_shine;
            uniform float alpha;

            uniform float colored;

            layout( location = 0 ) out vec4 FragColor;
            //out vec4 FragColor;

			void main()
			{
				vec4 n,halfV,viewV,ldir;
				float NdotL,NdotHV;
				vec4 color;

				if(colored == 0.0)
					color = texture(Tex1, TexCoord);
				else
					color = vec4(0,0,0,0);

				/* a fragment shader can't write a varying variable, hence we need
				a new variable to store the normalized interpolated normal */
				n = normalize(Normal);
		
				/* compute the dot product between normal and normalized lightdir */
				NdotL = max(dot(n,normalize(vLE)),0.0);
	
				if (NdotL > 0.0) {
					color += vec4(material_kd * NdotL + material_ka, 0);
		
					halfV = normalize(vLE + Position);
					NdotHV = max(dot(n,halfV),0.0);
					color += vec4(material_ks * pow(NdotHV,material_shine), 1);
				}
	
				FragColor = color;
			}

            
        ";
        #endregion
    }
}
