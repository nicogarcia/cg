using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace Utilities
{
    public abstract class Sweep
    {
        public PolyNet polynet = new PolyNet();

        public Sweep(Vector4[] face_vertices, Func<int, int, Matrix4> translation_step,
            Func<int, int, Matrix4> rotation_step, Func<int, int, Matrix4> scale_step, int steps)
        {
            polynet.addFace(face_vertices);

            face_vertices = new Vector4[]{
                face_vertices[0],
                face_vertices[3],
                face_vertices[2],
                face_vertices[1],
            };

            Vector4[] currentFace = face_vertices;
            Vector4[] nextFace = new Vector4[4];

            // For each division
            for (int i = 0; i < steps; i++)
            {
                Matrix4 transform = translation_step(i, steps) * rotation_step(i, steps) * scale_step(i, steps);
                
                // Generate next section
                nextFace = new Vector4[]{
                    Vector4.Transform(face_vertices[0], transform),
                    Vector4.Transform(face_vertices[1], transform),
                    Vector4.Transform(face_vertices[2], transform),
                    Vector4.Transform(face_vertices[3], transform),
                };

                // Generate Faces
                for (int j = 0; j < 4; j++)
                {
                    Vector4[] v = new Vector4[]{
                        currentFace[j],
                        currentFace[(j + 1) % 4],
                        nextFace[(j + 1) % 4],
                        nextFace[j],
                    };
                    polynet.addFace(v);
                }

                currentFace = nextFace;
            }
        
        }
    }
}
