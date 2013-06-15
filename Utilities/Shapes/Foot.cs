using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;
using Utilities.Shaders;
using OpenTK.Graphics;

namespace Utilities
{
    public class Foot : Sweep
    {

        public Foot(Vertex[] face_vertices, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step, int steps, ProgramObject program)
            : base(steps, program)
        {
            Vector2[][] textures = new Vector2[face_vertices.Length][];
            for (int i = 0; i < face_vertices.Length; i++)
            {
                textures[i] = new Vector2[4];
                textures[i][0] = new Vector2((i % 2) / 2, 0);
                textures[i][1] = new Vector2((i % 2) / 2 + 0.5f, 0);
                textures[i][2] = new Vector2((i % 2) / 2, 1f);
                textures[i][3] = new Vector2((i % 2) / 2 + 0.5f, 1f);
            }
            createSweep(face_vertices, translation_step, rotation_step, scale_step, textures);
            /*
            Vertex[][] toDraw = new Vertex[firstFace.Length + 2][];
            //indices = new int[firstFace.Length + 2];
            //count = new int[firstFace.Length + 2];

            // agregar caras extrudadas
            for (int i = 0; i < firstFace.Length; i++)
            {
                int messi = 0;
                toDraw[i] = new Vertex[(2 + 2 * steps)*4];
                HalfEdge current = polynet.halfEdges[firstFace[i]][firstFace[(i + 1) % firstFace.Length]];

                // first
                toDraw[i][messi++] = current.origin.position;

                for (int j = 0; j < steps; j++)
                {
                    // right (all parameters)
                    toDraw[i][messi++] = current.next.origin.position;
                    // left
                    toDraw[i][messi++] = current.prev.origin.position;

                    current = polynet.halfEdges[current.prev.origin][current.next.next.origin];

                }
                // latest
                toDraw[i][messi++] = current.next.next.origin.position;

            }*/

        }
    }
}
