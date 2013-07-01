using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Utilities.Shaders;
using OpenTK.Graphics.OpenGL;
using System.Drawing;

namespace Utilities
{
    public abstract class Sweep : Drawable3D
    {
        public PolyNet polynet = new PolyNet();
        public Vertex[] firstFace, tapa;
		public bool wire_mode = false;

        int steps;
        Vector2[][] textures;
        Func<int, int, Matrix4> translation_step;
        
        public Sweep(int steps, ProgramObject program) :
            base(program)
        {
            this.steps = steps;
        }

        public void createSweep(Vertex[] face_vertices, Vector4 color, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step,Vector2[][] textures)
        {
            this.translation_step = translation_step;
            this.textures = textures;

            // Generate first face reversing input vertex order
            Vertex[] backwards = new Vertex[face_vertices.Length];
            backwards[0] = face_vertices[0];
            for (int i = 1; i < backwards.Length; i++)
                backwards[i] = face_vertices[backwards.Length - i];
            firstFace = backwards;

            // Add first face (input reversed)
            polynet.addFace(firstFace);

            Vertex[] currentFace = face_vertices;
            Vertex[] nextFace = new Vertex[face_vertices.Length];

            // For each division
            for (int i = 0; i < steps; i++)
            {
                Matrix4 transform = translation_step(i, steps) * scale_step(i, steps);

                // Generate next section
                nextFace = new Vertex[face_vertices.Length];
                for (int j = 0; j < face_vertices.Length; j++)
                {
                    Vertex new_vertex = new Vertex(Vector4.Transform(face_vertices[j].position, transform), color, face_vertices[j].texture);
                    nextFace[j] = new_vertex;
                }

                // Generate Faces
                for (int j = 0; j < face_vertices.Length; j++)
                {
                    Vertex[] v = new Vertex[]{
                        currentFace[j],
                        currentFace[(j + 1) % face_vertices.Length],
                        nextFace[(j + 1) % face_vertices.Length],
                        nextFace[j],
                    };
                    polynet.addFace(v);
                }

                currentFace = nextFace;
            }
            tapa = currentFace;
            polynet.addFace(tapa);

            // Fill ArrayBuffer
            triangulate();
            base.fillArrayBuffer(true);
        }

        public void triangulate()
        {

            indices = new int[firstFace.Length + 2];
            count = new int[firstFace.Length + 2];

            // Add Base and top
            List<Vertex> Top = new List<Vertex>();
            List<Vertex> Base = new List<Vertex>();

            for (int i = 0; i < tapa.Length; i++)
                tapa[i].normal = new Vector4(0, 0, 1f, 0f);

            for (int i = 0; i < firstFace.Length; i++)
                firstFace[i].normal = new Vector4(0, 0, -1f, 0f);

            Top.Add(new Vertex(tapa[0]));
            Base.Add(new Vertex(firstFace[0]));

            int len = firstFace.Length + 1;

            for (int i = 1; i < len / 2; i++)
            {
                Base.Add(new Vertex(firstFace[i]));
                Top.Add(new Vertex(tapa[i]));

                Base.Add(new Vertex(firstFace[firstFace.Length - i]));
                Top.Add(new Vertex(tapa[firstFace.Length - i]));
            }

            if (len % 2 != 0)
            { // Face has odd number of vertices
                Base.Add(new Vertex(firstFace[firstFace.Length / 2]));
                Top.Add(new Vertex(tapa[firstFace.Length / 2]));
            }

            // Generate i triangle strips
            /*
                 * 2------------3
                 * -            -
                 * 0------------1
            */
            List<Vertex> vertexList = new List<Vertex>();
            int verticesLastCount = 0;
            for (int i = 0; i < firstFace.Length; i++)
            {
                // Get HalfEdge from i to the next
                HalfEdge current = polynet.halfEdges[firstFace[i]][firstFace[(i + 1) % firstFace.Length]].twin;

                TextureMapper.normal = current.face.normal;
                TextureMapper.textureExtents = textures[i];
                TextureMapper.faceExtents = faceExtents(firstFace[i], firstFace[(i + 1) % firstFace.Length]);

                indices[i] = vertexList.Count;
                // Start from vertex i (origin of current half edge)
                Vertex first = current.origin;
                first.texture = TextureMapper.map(current.origin.position);
                first.normal = new Vector4(polynet.normal(first));

                vertexList.Add(new Vertex(first));

                // Alternate between left and right, "steps" times
                for (int j = 0; j < steps; j++)
                {
                    // Right vertex
                    Vertex right = current.next.origin;
                    right.texture = TextureMapper.map(right.position);
                    right.normal = new Vector4(polynet.normal(right));
                    vertexList.Add(new Vertex(right));

                    // Left vertex
                    Vertex left = current.prev.origin;
                    left.texture = TextureMapper.map(left.position);
                    left.normal = new Vector4(polynet.normal(left));
                    vertexList.Add(new Vertex(left));

                    // Advance
                    current = polynet.halfEdges[current.next.next.origin][left].twin;
                }

                // Last vertex
                Vertex last = current.next.origin;
                last.texture = TextureMapper.map(last.position);
                last.normal = new Vector4(polynet.normal(last));
                vertexList.Add(new Vertex(last));

                count[i] = vertexList.Count - indices[i];
                verticesLastCount = vertexList.Count;
            }

            
            // Add vertices to "toDraw" single linear array
            indices[indices.Length - 2] = vertexList.Count;
            count[indices.Length - 2] = Base.Count;
            vertexList.AddRange(Base);

            indices[indices.Length - 1] = vertexList.Count;
            count[indices.Length - 1] = Top.Count;
            vertexList.AddRange(Top);

            toDraw = vertexList.ToArray();
        }

        /*
         * 2 ---- 3
         * |      |
         * 0 ---- 1
        */
        private Vector4[] faceExtents(Vertex vertex, Vertex vertex_2)
        {
            Matrix4 tr = translation_step(steps, steps);

            return new Vector4[]
            {
                vertex.position,
                vertex_2.position,
                Vector4.Transform(vertex.position, tr),
                Vector4.Transform(vertex_2.position, tr)
            };

        }

        public override void paint(Matrix4 projMatrix, Matrix4 modelViewMatrix)
        {

            GL.UseProgram(program.program_handle);

            modelViewMatrix = transformation * modelViewMatrix;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(modelViewMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref modelViewMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);

			GL.Uniform4(light_position_location, light.position.x, light.position.y, light.position.z, 1f);

            // Light Intensity?
            GL.Uniform3(light_intensity_location, 0.5f, 0.5f, 0.5f);

            // Silver ADSs
            // 0.19225	0.19225	0.19225	0.50754	0.50754	0.50754	0.508273	0.508273	0.508273	0.4
            GL.Uniform3(material_ka_location, 0.519225f, 0.519225f, 0.519225f);
            GL.Uniform3(material_kd_location, 0.50754f, 0.50754f, 0.50754f);
            GL.Uniform3(material_ks_location, 0.508273f, 0.508273f, 0.508273f);
            GL.Uniform1(material_shine_location, 51.2f);

            GL.Uniform1(colored_location, colored ? 1.0f : 0f);
			GL.Uniform1(noise_location, noise_texture ? 1 : 0);
			GL.Uniform1(illumination_model_location, illumination_model);
			GL.Uniform1(roughness_location, 0.3f);
			GL.Uniform1(reflect_location, 1f);
            
            GL.BindVertexArray(VAO_ID);
            if(wire_mode)
				GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            GL.DrawElements(BeginMode.Triangles, ebo_array.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

            GL.BindVertexArray(0);

            if (draw_normals)
            {
                GL.BindVertexArray(NVAO_ID);

                GL.DrawElements(BeginMode.Lines, normals_ebo_array.Length, DrawElementsType.UnsignedInt, IntPtr.Zero);

                GL.BindVertexArray(0);
            }

            GL.UseProgram(0);
        }

    }

    class TextureMapper
    {
        public static Vector2[] textureExtents;
        public static Vector4[] faceExtents;
        public static Vector3 normal;

        public static Vector4 map(Vector4 position)
        {
            Vector4 w = faceExtents[1] - faceExtents[0];
            Vector4 h = faceExtents[2] - faceExtents[0];

            Vector4 proj_pos = position - Vector4.Dot(position - faceExtents[0], new Vector4(normal)) * new Vector4(normal);
            
            proj_pos -= faceExtents[0];

            float ratio_w = Vector4.Dot(proj_pos, w) / w.LengthSquared;
            float ratio_h = Vector4.Dot(proj_pos, h) / h.LengthSquared;

            float texture_width = textureExtents[1].X - textureExtents[0].X;
            float texture_height = textureExtents[2].Y - textureExtents[0].Y;
            
            if (texture_width > 1)
                throw new NotImplementedException();

            Vector4 texCoord = new Vector4(textureExtents[0].X + ratio_w * texture_width, textureExtents[0].Y + ratio_h * texture_height, 0f, 0f);

            //Console.WriteLine(texCoord);
            /*
            Random r = new Random();
            texCoord = new Vector4((float) r.NextDouble(), (float) r.NextDouble(),0f, 0f);*/
            return texCoord;
        }
    }
}
