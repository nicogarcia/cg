using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Utilities
{
    public class PolyNet
    {
        public List<Face> faces;
        public Dict<Vector4,HalfEdge> halfEdges;

        public PolyNet()
        {
            faces = new List<Face>();
            halfEdges = new Dict<Vector4, HalfEdge>();
        }

        public void addFace(Vector4[] vertices)
        {
            HalfEdge[] halfEdges = new HalfEdge[vertices.Length];
            //add Half Edge
            for (int curr = 0; curr < vertices.Length; curr++)
            {
                int next = (curr + 1) % vertices.Length;
                HalfEdge exist = existHalfEdge(vertices[curr], vertices[next]);
                if (exist == null)
                {
                    //create edge
                    halfEdges[curr] = new HalfEdge(vertices[curr], vertices[next]);
                    this.halfEdges.Add(vertices[curr], vertices[next], halfEdges[curr]);
                }
                else
                {
                    halfEdges[curr] = exist;
                }
            }

            Face face = new Face(halfEdges[0]);
            faces.Add(face);

            for(int i = 0; i < halfEdges.Length; i++){
                int next = (i + 1) % halfEdges.Length;
                int prev = (halfEdges.Length + i - 1) % halfEdges.Length;

                halfEdges[i].next = halfEdges[next];
                halfEdges[i].prev = halfEdges[prev];
            }
        }

        private HalfEdge existHalfEdge(Vector4 origin, Vector4 dest)
        {
            if(halfEdges.ContainsKey(origin) && halfEdges[origin].ContainsKey(dest))
                return halfEdges[origin][dest];
            return null;
        }

    }

    public class Face
    {
        public HalfEdge halfEdge;

        public Face(HalfEdge halfEdge)
        {
            this.halfEdge = halfEdge;
        }

        public Vector4[] vertices()
        {
            List<Vector4> list = new List<Vector4>();
            HalfEdge current = halfEdge;

            do
            {
                list.Add(current.origin);
                current = current.next;
            } while (current != halfEdge);
            return list.ToArray();
        }
    }

    public class HalfEdge
    {
        public Vector4 origin;
        public HalfEdge twin;
        public HalfEdge prev, next;
        public Face face;

        public HalfEdge(Vector4 origin, Vector4 dest)
        {
            this.origin = origin;
            twin = new HalfEdge();
            twin.origin = dest;
            twin.twin = this;
        }

        public HalfEdge()
        {
        }
    }
}
