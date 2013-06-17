using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Utilities.Shaders;

namespace Utilities
{
    public class Cylinder : Sweep
    {
        public Cylinder(float radius, float height, int faces, Vector4 color, ProgramObject program) : base(1, program)
        {
            float step = 2 * (float)Math.PI / faces;
            float theta = (float) Math.PI / 4;

            Vertex[] base_vertices = new Vertex[faces];

            for (int i = 0; i < faces; i++, theta += step)
            {
                float x = radius * (float)Math.Cos(theta);
                float y = radius * (float)Math.Sin(theta);
                base_vertices[i] = new Vertex(x, y, 0, 1f);
                base_vertices[i].color = color;

                // Bottom normal pointing down and its texture
                base_vertices[i].normal = new Vector4(x, y, -100f, 0f);
                base_vertices[i].texture = new Vector4(new Vector2((184 + 128 * (float)Math.Cos(theta)) / 500, (188 + 128 * (float)Math.Sin(theta)) / 375));
                //Console.WriteLine(base_vertices[i].texture.ToString());
            }

            
            Vector2[][] textures = new Vector2[faces][];
            float mapping_step = (float) 1 / faces;
            Console.WriteLine("Start");
            for (int i = 0; i < faces; i++)
            {
                textures[i] = new Vector2[4];
                textures[i][0] = new Vector2(mapping_step * i, 0);
                textures[i][1] = new Vector2(mapping_step * (i + 1), 0);
                textures[i][2] = new Vector2(mapping_step * i, 1f);
                textures[i][3] = new Vector2(mapping_step * (i + 1), 1f);
                Console.WriteLine(textures[i].ToString());
            }
            Console.WriteLine("Finish");
            createSweep(
                base_vertices,
                color,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0f, 0f, height));
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
                ),
                textures
             );
        }

    }
}
