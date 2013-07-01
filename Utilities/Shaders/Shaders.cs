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
            layout (location = 0) in vec4 position;
            layout (location = 1) in vec4 input_color;
            //in vec4 position;
            //in vec4 input_color;
            out vec4 fragment_input_color;
            void main()
            { 
                gl_Position = position;

                fragment_input_color = input_color;
            }"; 
        #endregion

        #region V_TRANSF
        public const string VERTEX_TRANSFORMATION_SHADER = @"
            #version 330
            layout (location = 0) in vec4 position;
            layout (location = 1) in vec4 input_color;
            //in vec4 position;
            //in vec4 input_color;
            uniform mat4 projectionMatrix;
            uniform mat4 modelView;
			out vec4 fragment_input_color;
            void main(){
                gl_Position = projectionMatrix * modelView * position;

                float rojo = 1.0 - abs(position.x);
                float verde = 1.0 - abs(position.y);
                float azul = abs(position.x + position.y);

                fragment_input_color = vec4(rojo, verde, azul, 1.0);
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
                vec3 material_ka; // Ambient reflectivity
                vec3 material_kd; // Diffuse reflectivity
                vec3 material_ks; // Specular reflectivity
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

            layout(location = 0) out vec4 oColor;  

            uniform vec3 material_ka;
            uniform vec3 material_kd;
            uniform vec3 material_ks;
            uniform float material_shine;   
            uniform float fRoughness;
            uniform float reflecAtNormalIncidence;
            uniform int illum_model;
            uniform int noise;
            uniform float alpha;

            uniform float colored;

            float rand(vec2 co){
				return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
            }

            void main()
            {
                //calcular los distintos vectores y normalizarlos
                vec3 N = vec3(normalize(Normal));
                vec3 L = vec3(normalize(vLE));
                vec3 R = reflect(L, N);
                vec3 V = vec3(normalize(-Position));
                vec3 H = normalize(L + V);
                vec4 texColor;
				
                if(noise==0){
					if(colored == 0)
						texColor = texture(Tex1, TexCoord);
					else
						texColor = Color;
                }else
                    texColor=vec4(rand(TexCoord));

                // Blinn-Phong
                if(illum_model == 0)
                {
                    //calculo termino difuso+especular de Blinn-Phong
                    float difuso = max(dot(L,N), 0.0);
                    float especBlinngPhong = pow(max(dot(N, H), 0.0), material_shine);
                    if(dot(L,N) < 0.0)
                        especBlinngPhong = 0.0;
                    oColor = vec4(material_ka + material_kd * difuso, 1)* texColor  + vec4(material_ks, 1) * especBlinngPhong;
                }

                // Cook-Torrence
                if(illum_model == 1)
                {
                    //factor de atenuacion geometrica G
                    float denom = dot(V, H);
                    float NdotH = dot(N, H);
                    float NdotV = dot(N, V);
                    float NdotL = dot(N, L);
                    float Ge = (2.0 * NdotH * NdotV) / denom;
                    float Gs = (2.0 * NdotH * NdotL) / denom;
                    float G = min(1.0f, min(Ge, Gs));

                    //funcion de distribucion de las microfacetas D
                    float HdotN = dot(H,N); //cos(beta)                    
                    float b = acos(HdotN); //angulo entre H y N
                    float exponente = -pow((tan(b)/fRoughness), 2.0);
                    float terminoDivision = 1.0 / (4.0 * pow(fRoughness, 2.0) * pow(HdotN, 4.0));
                    float D = terminoDivision * exp(exponente);
                    
                    //término de Freshnel
                    float F = reflecAtNormalIncidence + (1.0 - reflecAtNormalIncidence)*pow((1.0 - dot(H,V)),5.0);
        
                    float pi = 3.1415926;
                    float Rs = (F*D*G)/(pi * NdotV * NdotL);
                    oColor = vec4(material_ka, 1)* texColor + max(0.0, dot(L,N)) * vec4(material_kd, 1) * texColor + vec4(material_ks, 1) * Rs;
                }
                // Oren-Nayar
                if(illum_model==2)
                {
                    float alpha = max(acos(dot(V,N)), acos(dot(L,N)));
                    float beta = min(acos(dot(V,N)), acos(dot(L,N)));
                    float gamma = dot(V - N * dot(V,N), L - N * dot(L,N)); //cos(phiR - phiI)
                    float rough_sq = fRoughness * fRoughness; //(sigma al cuadrado)
                    float A = 1.0 - 0.5 * (rough_sq / (rough_sq + 0.33));
                    float B = 0.45 * (rough_sq / (rough_sq + 0.09));
                    oColor = vec4(material_ka, 1)*texColor + vec4(material_kd, 1)*texColor* max(dot(N, L), 0.0) * (A + B * max(gamma, 0.0) * sin(alpha)*tan(beta));
                }              
            }";
			/*
			
			
			@"
            #version 140
            in vec4 Position;
            in vec4 Normal;
            in vec4 Color;
            in vec2 TexCoord;
            in vec4 vLE;            

            uniform sampler2D Tex1;

            uniform vec3 light_intensity;
            uniform vec3 material_material_ka;
            uniform vec3 material_material_kd;
            uniform vec3 material_material_ks;
            uniform float material_shine;
            uniform float alpha;

            uniform float colored;
			uniform float noise;
			uniform float illum_model;

            layout( location = 0 ) out vec4 FragColor;
            //out vec4 FragColor;

			float rand(vec2 co){
				return fract(sin(dot(co.xy ,vec2(12.9898,78.233))) * 43758.5453);
			}

			void main()
			{
				vec3 N = vec3(normalize(Normal));
                vec3 L = vec3(normalize(vLE));
                vec3 R = reflect(L, N);
                vec3 V = vec3(normalize(Position));
                vec3 H = normalize(L + V);

				vec4 color;
				vec4 texColor;

				if(colored == 0.0){
					if(noise > 0)
						texColor = vec4(rand(TexCoord));
					else
						texColor = texture(Tex1, TexCoord);
				}else
					texColor = vec4(0,0,0,0);

                //illum_model Blinn-Phong
                if(illum_model == 0)
                {
                    //calculo termino difuso+especular de Blinn-Phong
                    float difuso = max(dot(L,N), 0.0);
                    float especBlinngPhong = pow(max(dot(N, H), 0.0), material_shine);
                    if(dot(L,N) < 0.0)
                        especBlinngPhong = 0.0;
                    color = vec4((material_material_ka + material_material_kd * difuso)* texColor, 1)  + vec4(material_material_ks * especBlinngPhong, 1);
                }

                // Cook-Torrence
                if(illum_model == 1)
                {
                    float denom = dot(V, H);
                    float NdotH = dot(N, H);
                    float NdotV = dot(N, V);
                    float NdotL = dot(N, L);
                    float Ge = (2.0 * NdotH * NdotV) / denom;
                    float Gs = (2.0 * NdotH * NdotL) / denom;
                    float G = min(1.0f, min(Ge, Gs));

                    float HdotN = dot(H,N); //cos(beta)                    
                    float b = acos(HdotN); //angulo entre H y N
                    float exponente = -pow((tan(b)/fRoughness), 2.0);
                    float terminoDivision = 1.0 / (4.0 * pow(fRoughness, 2.0) * pow(HdotN, 4.0));
                    float D = terminoDivision * exp(exponente);
                    
                    float F = reflecAtNormalIncidence + (1.0 - reflecAtNormalIncidence)*pow((1.0 - dot(H,V)),5.0);
        
                    float pi = 3.1415926;
                    float Rs = (F*D*G)/(pi * NdotV * NdotL);
                    color = material_material_ka * texColor + max(0.0, dot(L,N)) * (material_material_kd * texColor + material_material_ks * Rs);
                }

                //Oren-Nayar
                if(illum_model == 2)
                {
                    float alpha = max(acos(dot(V,N)), acos(dot(L,N)));
                    float beta = min(acos(dot(V,N)), acos(dot(L,N)));
                    float gamma = dot(V - N * dot(V,N), L - N * dot(L,N)); //cos(phiR - phiI)
                    float rough_sq = fRoughness * fRoughness; //(sigma al cuadrado)
                    float A = 1.0 - 0.5 * (rough_sq / (rough_sq + 0.33));
                    float B = 0.45 * (rough_sq / (rough_sq + 0.09));
                    color = material_material_ka*texColor + material_material_kd*texColor* max(dot(N, L), 0.0) * (A + B * max(gamma, 0.0) * sin(alpha)*tan(beta));
                }      
	
				FragColor = color;
			}

            
        ";*/
        #endregion
    }
}
