﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using Utilities.Shaders;

namespace Utilities
{
    public class Cylinder : Sweep
    {
        public Cylinder(float radius, int faces, ProgramObject program) : base(1, program)
        {
            float step = 2 * (float)Math.PI / faces;
            float theta = 0;

            Vertex[] base_vertices = new Vertex[faces];

            for (int i = 0; i < faces; i++, theta += step)
            {
                float x = radius * (float)Math.Cos(theta);
                float y = radius * (float)Math.Sin(theta);
                base_vertices[i] = new Vertex(x, y, -radius * 2, 1f);

                // Bottom normal pointing down and its texture
                base_vertices[i].normal = new Vector4(x, y, -100f, 0f);
                base_vertices[i].texture = new Vector4(new Vector2((184 + 128 * base_vertices[i].position.X) / 500, (188 + 128 * base_vertices[i].position.Y) / 375));
            
            }
            
            Vector2[][] textures = new Vector2[faces][];
            float mapping_step = (float) 1 / faces;
            for (int i = 0; i < faces; i++)
            {
                textures[i] = new Vector2[4];
                textures[i][0] = new Vector2(mapping_step * i, 0);
                textures[i][1] = new Vector2(mapping_step * (i + 1), 0);
                textures[i][2] = new Vector2(mapping_step * i, 1f);
                textures[i][3] = new Vector2(mapping_step * (i + 1), 1f);
            }

            createSweep(
                base_vertices,
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0f, 0f, radius * 4f));
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
