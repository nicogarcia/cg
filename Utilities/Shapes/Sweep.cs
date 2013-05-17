using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Utilities.Shaders;
using OpenTK.Graphics.OpenGL;

namespace Utilities
{
    public abstract class Sweep : Drawable3D
    {
        public PolyNet polynet = new PolyNet();
        public Vertex[] firstFace, tapa;
        int[] indices, count;
        int steps;

        public Sweep(int steps, ProgramObject program) :
            base(program, BeginMode.TriangleStrip)
        {
            this.steps = steps;
        }

        public void createSweep(Vertex[] face_vertices, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step)
        {
            polynet.addFace(face_vertices);

            Vertex[] backwards = new Vertex[face_vertices.Length];
            backwards[0] = face_vertices[0];
            for (int i = 1; i < backwards.Length; i++)
            {
                backwards[i] = face_vertices[backwards.Length - i];
            };

            firstFace = backwards;

            Vertex[] currentFace = backwards;
            Vertex[] nextFace = new Vertex[backwards.Length];

            // For each division
            for (int i = 0; i < steps; i++)
            {
                Matrix4 transform = translation_step(i, steps) * scale_step(i, steps);

                // Generate next section
                nextFace = new Vertex[backwards.Length];
                for (int j = 0; j < backwards.Length; j++)
                {
                    nextFace[j] = new Vertex(Vector4.Transform(backwards[j].position, transform));
                }

                // Generate Faces
                for (int j = 0; j < backwards.Length; j++)
                {
                    Vertex[] v = new Vertex[]{
                        currentFace[j],
                        currentFace[(j + 1) % backwards.Length],
                        nextFace[(j + 1) % backwards.Length],
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
            base.fillArrayBuffer();
        }
        public void triangulate()
        {
            Vertex[][] toDrawAux = new Vertex[firstFace.Length + 2][];
            indices = new int[firstFace.Length + 2];
            count = new int[firstFace.Length + 2];
            //agregar base y tapa

            toDrawAux[firstFace.Length] = new Vertex[firstFace.Length];
            toDrawAux[firstFace.Length + 1] = new Vertex[firstFace.Length];

            int top = firstFace.Length + 1;

            int cursor = 1;
            toDrawAux[firstFace.Length][0] = firstFace[0];
            toDrawAux[firstFace.Length + 1][0] = tapa[0];
            for (int i = 1; i < top / 2; i++)
            {
                toDrawAux[firstFace.Length][cursor] = firstFace[i];
                toDrawAux[firstFace.Length + 1][cursor++] = tapa[i];
                toDrawAux[firstFace.Length][cursor] = firstFace[firstFace.Length - i];
                toDrawAux[firstFace.Length + 1][cursor++] = tapa[firstFace.Length - i];
            }

            if (top % 2 != 0)
            {
                toDrawAux[firstFace.Length][cursor] = firstFace[firstFace.Length / 2];
                toDrawAux[firstFace.Length + 1][cursor] = tapa[firstFace.Length / 2];
            }

            // Generate i triangle strips
            for (int i = 0; i < firstFace.Length; i++)
            {
                int messi = 0;
                toDrawAux[i] = new Vertex[2 + 2 * steps];

                // Get HalfEdge from i to the next
                HalfEdge current = polynet.halfEdges[firstFace[i]][firstFace[(i + 1) % firstFace.Length]];

                // Start from vertex i (origin of current half edge)
                toDrawAux[i][messi++] = current.origin;

                // Alternate between left and right, "steps" times
                for (int j = 0; j < steps; j++)
                {
                    // Left vertex
                    toDrawAux[i][messi++] = current.next.origin;
                    // Right vertex
                    toDrawAux[i][messi++] = current.prev.origin;

                    // Advance
                    current = polynet.halfEdges[current.prev.origin][current.next.next.origin];
                }

                // Last vertex
                toDrawAux[i][messi++] = current.next.next.origin;

            }

            // Generate the single array with vertices
            toDraw = new Vertex[2 * firstFace.Length * (2 + steps)];
            int roman = 0;
            for (int i = 0; i < toDrawAux.Length; i++)
            {
                indices[i] = roman;
                for (int j = 0; j < toDrawAux[i].Length; j++)
                {
                    toDrawAux[i][j].normal = new Vector4(polynet.normal(toDrawAux[i][j]));
                    toDraw[roman++] = toDrawAux[i][j];
                }
                count[i] = toDrawAux[i].Length;
            }
        }

        public override void paint(Matrix4 projMatrix, Matrix4 zoomMatrix)
        {

            GL.UseProgram(program.program_handle);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            projMatrix = projMatrix * transformation;
            Matrix4 normalMatrix = Matrix4.Invert(Matrix4.Transpose(zoomMatrix));

            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);
            GL.UniformMatrix4(normal_location, false, ref normalMatrix);
            
            GL.Uniform4(light_position_location, 3.0f, 3.0f, 3.0f, 1f);
            // Light Intensity?
            GL.Uniform3(material_ka_location, 0.10f, 0.19f, 0.17f);
            GL.Uniform3(material_kd_location, 0.40f, 0.74f, 0.69f);
            GL.Uniform3(material_ks_location, 0.30f, 0.31f, 0.31f);
            GL.Uniform1(material_shine_location, 12.8f);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.MultiDrawArrays(BeginMode.TriangleStrip, indices, count, count.Length);

            GL.UseProgram(0);
        }



    }
}
