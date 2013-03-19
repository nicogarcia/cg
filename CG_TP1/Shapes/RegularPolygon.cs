﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace CG_TP1
{
    class RegularPolygon : Drawable
    {
        public RegularPolygon(int edges, float radius, bool solid)
        {
            int i = 0;

            float step = 2 * (float)Math.PI / edges;
            BeginMode mode;
            vertices = new Vector4[edges];

            if (solid)
            {
                mode = BeginMode.Polygon;
            }
            else
            {
                mode = BeginMode.LineLoop;
            }

            for (float theta = (float) Math.PI / 2; i < edges; i++, theta += step)
                vertices[i] = new Vector4(radius * (float)Math.Cos(theta), radius * (float)Math.Sin(theta), 0f, 1f);

            GLManager.addDrawable(this, mode);
        }
    }
}
