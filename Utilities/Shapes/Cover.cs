using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities
{
    public class Cover
    {
        public PolyNet polynet = new PolyNet();

        public Cover(Vertex[] bottom)
        {
            polynet.addFace(bottom);


            Vertex[] nextFace = new Vertex[8];

            float scale_step = 1.05f;

            Matrix4 transform = Matrix4.Scale(scale_step, scale_step, 1f) * Matrix4.CreateTranslation(new Vector3(0, 0, 0.1f));

            // Generate next section
            nextFace = new Vertex[]{
                    new Vertex(Vector4.Transform(bottom[0].position,transform)),
                    new Vertex(Vector4.Transform(bottom[1].position,transform)),
                   new Vertex( Vector4.Transform(bottom[2].position,transform)),
                    new Vertex(Vector4.Transform(bottom[3].position,transform)),
                   new Vertex( Vector4.Transform(bottom[4].position,transform)),
                   new Vertex( Vector4.Transform(bottom[5].position,transform)),
                   new Vertex( Vector4.Transform(bottom[6].position,transform)),
                    new Vertex(Vector4.Transform(bottom[7].position,transform)),
                };
            polynet.addFace(nextFace);

            // Generate Faces
            for (int j = 0; j < 8; j++)
            {
                Vertex[] v = new Vertex[]{
                        new Vertex(bottom[j].position),
                        new Vertex(bottom[(j + 1) % 8].position),
                        new Vertex(nextFace[(j + 1) % 8].position),
                        new Vertex(nextFace[j].position),
                    };
                polynet.addFace(v);
            }
        }

    }
}
