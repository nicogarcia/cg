﻿using System;
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

        public Foot(float height, int num_steps, ProgramObject program)
            : base(num_steps, program)
        {
            Vertex[] vertices = new Vertex[]{
                new Vertex(new Vector4(1f, -1f, 0, 1f)),
                new Vertex(new Vector4(1f, 1f, 0, 1f)),
                new Vertex(new Vector4(-1f, 1f, 0, 1f)),
                new Vertex(new Vector4(-1f, -1f, 0, 1f)),
            };

            Vector2[][] textures = new Vector2[vertices.Length][];
            for (int i = 0; i < vertices.Length; i+=2)
            {
                textures[i] = new Vector2[4];
                textures[i][0] = new Vector2((i % 2) / 2, 0);
                textures[i][1] = new Vector2((i % 2) / 2 + 0.75f, 0);
                textures[i][2] = new Vector2((i % 2) / 2, 1f);
                textures[i][3] = new Vector2((i % 2) / 2 + 0.75f, 1f);
            }

			for (int i = 1; i < vertices.Length; i+= 2)
			{
				textures[i] = new Vector2[4];
				textures[i][0] = new Vector2((i % 2) / 2 + 0.75f, 0);
				textures[i][1] = new Vector2(1f, 0);
				textures[i][2] = new Vector2((i % 2) / 2 + 0.75f, 1f);
				textures[i][3] = new Vector2(1f, 1f);
			}

            float height_step = height / num_steps;

            createSweep(vertices, new Vector4(0, 1f, 0, 1f),
                new Func<int, int, Matrix4>(
                    delegate(int current, int steps)
                    {
                        return Matrix4.CreateTranslation(new Vector3(0, 0, (current + 1) * height_step));
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
                        current++;

                        float pend = 4 * 0.5f / (float)Math.Pow(steps * 0.1f, 2);
                        float scale_factor = (float)(pend * Math.Pow(current * 0.1f - steps * 0.1f / 2f, 2) + 0.5f);
                        return Matrix4.Scale(scale_factor, scale_factor, 1f);
                    }
                )
                , textures);

        }
    }
}
