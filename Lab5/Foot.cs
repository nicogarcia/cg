using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5
{
    class Foot
    {

        public PolyNet polynet = new PolyNet();

        public Foot(float height, int divisions, Vector4[] face_vertices)
        {

            polynet.addFace(face_vertices);

            float step = height / divisions;
            float width = (face_vertices[0] - face_vertices[1]).Length;
            float pend = 9 / 4f * width / (float) Math.Pow(height, 1);

            Vector4[] currentFace = face_vertices;
            Vector4[] nextFace = new Vector4[4];

            // For each division
            for (int i = 0; i < divisions; i++)
            {

              float scale_step = (float) ((i + 1) * step / height + width / 1.85f);

                Matrix4 transform = Matrix4.Scale(scale_step, scale_step, 1f) * Matrix4.CreateTranslation(new Vector3(0, 0, step));
                
                // Generate next section
                nextFace = new Vector4[]{
                    Vector4.Transform(currentFace[0],transform),
                    Vector4.Transform(currentFace[1],transform),
                    Vector4.Transform(currentFace[2],transform),
                    Vector4.Transform(currentFace[3],transform),
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
