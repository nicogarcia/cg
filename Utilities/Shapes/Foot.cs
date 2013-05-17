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
            createSweep(face_vertices, translation_step, rotation_step, scale_step);
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
