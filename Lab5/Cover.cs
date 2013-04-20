using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Lab5
{
    class Cover
    {
        public PolyNet polynet = new PolyNet();

        public Cover(Vector4[] bottom)
        {
            polynet.addFace(bottom);


            Vector4[] nextFace = new Vector4[8];

            float scale_step = 1.05f;

            Matrix4 transform = Matrix4.Scale(scale_step, scale_step, 1f) * Matrix4.CreateTranslation(new Vector3(0, 0, 0.1f));

            // Generate next section
            nextFace = new Vector4[]{
                    Vector4.Transform(bottom[0],transform),
                    Vector4.Transform(bottom[1],transform),
                    Vector4.Transform(bottom[2],transform),
                    Vector4.Transform(bottom[3],transform),
                    Vector4.Transform(bottom[4],transform),
                    Vector4.Transform(bottom[5],transform),
                    Vector4.Transform(bottom[6],transform),
                    Vector4.Transform(bottom[7],transform),
                };
            polynet.addFace(nextFace);

            // Generate Faces
            for (int j = 0; j < 8; j++)
            {
                Vector4[] v = new Vector4[]{
                        bottom[j],
                        bottom[(j + 1) % 8],
                        nextFace[(j + 1) % 8],
                        nextFace[j],
                    };
                polynet.addFace(v);
            }
        }

    }
}
