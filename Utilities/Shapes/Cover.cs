using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utilities;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;

namespace Utilities
{
    public class Cover : Sweep
    {
        public Cover(float height, int num_steps, ProgramObject program)
            : base(num_steps, program)
        {
            float x = 2.0f;
            float y = 2.0f;
            float z = 2f;
            float aux = 0.15f;

            Vertex[] vertices = new Vertex[]{
               new Vertex(-x, -y + aux, z, 1.0f),
               new Vertex(-x + aux, -y, z, 1.0f),
               new Vertex(x - aux, -y, z, 1.0f),
               new Vertex(x, -y + aux, z, 1.0f),
               new Vertex(x, y - aux, z, 1.0f),
               new Vertex(x - aux, y, z, 1.0f),
               new Vertex(-x + aux, y, z, 1.0f),
               new Vertex(-x, y - aux, z, 1.0f)
            };

			float step = 2 * (float)Math.PI / vertices.Length;
			float theta = (float)Math.PI / 4;
			float radius = 2f;

			for (int i = 0; i < vertices.Length; i++, theta += step)
            {
                // Bottom normal pointing down and its texture
                vertices[i].texture = new Vector4(new Vector2((184 + 128 * (float)Math.Cos(theta)) / 500, (188 + 128 * (float)Math.Sin(theta)) / 375));
                //Console.WriteLine(base_vertices[i].texture.ToString());
            }


            Vector2[][] textures = new Vector2[vertices.Length][];
            for (int i = 0; i < vertices.Length; i++)
            {
                textures[i] = new Vector2[4];
                textures[i][0] = new Vector2((i % 2) / 2, 0);
                textures[i][1] = new Vector2((i % 2) / 2 + 0.5f, 0);
                textures[i][2] = new Vector2((i % 2) / 2, 1f);
                textures[i][3] = new Vector2((i % 2) / 2 + 0.5f, 1f);
            }

            float height_step = height / num_steps;

            createSweep(vertices, new Vector4(0, 1f, 0, 1f),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0, 0, height_step));
                    }
                ),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.Identity;
                    }
                ),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.Identity;
                    }
                )
                , textures);
        }

    }
}
