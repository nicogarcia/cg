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
        public Vector4[] firstFace, tapa;
        int [] indices,count;
        public Vector4[] draw;
        int steps;

        public Sweep(Vector4[] face_vertices, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step, int steps, ProgramObject program) :
            base(program, BeginMode.TriangleStrip)
        {
            this.steps = steps;
            polynet.addFace(face_vertices);

            Vector4[] backwards = new Vector4[face_vertices.Length];

            backwards[0] = face_vertices[0];

            for (int i = 1; i < backwards.Length; i++)
            {
                backwards[i] = face_vertices[backwards.Length - i];
            };

            firstFace = backwards;

            Vector4[] currentFace = backwards;
            Vector4[] nextFace = new Vector4[backwards.Length];

            // For each division
            for (int i = 0; i < steps; i++)
            {
                Matrix4 transform = translation_step(i, steps) * scale_step(i, steps);

                // Generate next section


                nextFace = new Vector4[backwards.Length];
                for (int j = 0; j < backwards.Length; j++)
                {
                    nextFace[j] = Vector4.Transform(backwards[j], transform);
                }

                // Generate Faces
                for (int j = 0; j < backwards.Length; j++)
                {
                    Vector4[] v = new Vector4[]{
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
            draw = triangulate();

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(draw.Length * Vector4.SizeInBytes),
                            draw, BufferUsageHint.StaticDraw);
        }

        public Vector4[] triangulate()
        {
            Vector4[][] toDraw = new Vector4[firstFace.Length + 2][];
            indices = new int[firstFace.Length + 2];
            count = new int[firstFace.Length + 2];
            //agregar base y tapa

            toDraw[firstFace.Length] = new Vector4[firstFace.Length];
            toDraw[firstFace.Length + 1] = new Vector4[firstFace.Length];

            int top = firstFace.Length + 1;

            int cursor = 1;
            toDraw[firstFace.Length][0] = firstFace[0];
            toDraw[firstFace.Length + 1][0] = tapa[0];
            for (int i = 1; i < top / 2; i++)
            {
                toDraw[firstFace.Length][cursor] = firstFace[i];
                toDraw[firstFace.Length + 1][cursor++] = tapa[i];
                toDraw[firstFace.Length][cursor] = firstFace[firstFace.Length - i];
                toDraw[firstFace.Length + 1][cursor++] = tapa[firstFace.Length - i];
            }

            if (top % 2 != 0)
            {
                toDraw[firstFace.Length][cursor] = firstFace[firstFace.Length / 2];
                toDraw[firstFace.Length + 1][cursor] = tapa[firstFace.Length / 2];
            }

            // agregar caras extrudadas
            for (int i = 0; i < firstFace.Length; i++)
            {
                int messi = 0;
                toDraw[i] = new Vector4[2 + 2 * steps];
                HalfEdge current = polynet.halfEdges[firstFace[i]][firstFace[(i + 1) % firstFace.Length]];

                toDraw[i][messi++] = current.origin;

                for (int j = 0; j < steps; j++)
                {
                    toDraw[i][messi++] = current.next.origin;
                    toDraw[i][messi++] = current.prev.origin;

                    current = polynet.halfEdges[current.prev.origin][current.next.next.origin];

                }

                toDraw[i][messi++] = current.next.next.origin;

            }
            Vector4[] toRet = new Vector4[2 * firstFace.Length *( 2 + steps)];
            int roman = 0;
            for (int i = 0; i < toDraw.Length; i++)
            {
                indices[i] = roman;
                for (int j = 0; j < toDraw[i].Length; j++)
                {
                    toRet[roman++] = toDraw[i][j];
                }
                count[i] = toDraw[i].Length;
            }

            return toRet;
        }

        public override void paint(Matrix4 projMatrix, Matrix4 zoomMatrix)
        {

            GL.UseProgram(program.program_handle);

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

            projMatrix = projMatrix * transformation;
            GL.UniformMatrix4(projection_location, false, ref projMatrix);
            GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);

            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

            GL.MultiDrawArrays(BeginMode.LineLoop, indices, count, count.Length);

            GL.UseProgram(0);
        }



    }
}
