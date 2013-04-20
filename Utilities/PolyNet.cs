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

            //add halfedges
            for (int curr = 0; curr < vertices.Length; curr++)
            {
                int next_he = (curr + 1) % vertices.Length;

                // look if half-edge already exists
                HalfEdge exist = existHalfEdge(vertices[curr], vertices[next_he]);

                // if it does, add it to the array, otherwise create it
                if (exist == null)
                {
                    // create half edge from current to next vertex
                    halfEdges[curr] = new HalfEdge(vertices[curr], vertices[next_he]);
                    // add halfedge to polynet halfedges dictionary
                    this.halfEdges.Add(vertices[curr], vertices[next_he], halfEdges[curr]);
                }
                else
                {
                    halfEdges[curr] = exist;
                }
            }

            // create face with some halfedge
            Face face = new Face(halfEdges[0]);
            // calculate face's normal
            face.normal = faceNormal(vertices);
            // add face to faces list
            faces.Add(face);

            // link halfedges with next and prev
            for(int i = 0; i < halfEdges.Length; i++){
                int next = (i + 1) % halfEdges.Length;
                int prev = (halfEdges.Length + i - 1) % halfEdges.Length;

                halfEdges[i].next = halfEdges[next];
                halfEdges[i].prev = halfEdges[prev];

                // link face to halfedge
                halfEdges[i].face = face;
            }
        }

        private Vector3 faceNormal(Vector4[] vertices)
        {
            Vector3 edge1, edge2;

            edge1 = new Vector3(vertices[1] - vertices[0]);
            edge2 = new Vector3(vertices[2] - vertices[1]);

            // get face's normal vector with cross product
            Vector3 ret = Vector3.Cross(edge1, edge2);
            return ret;
        }

        // gets vertex normal as an average of faces normals
        public Vector3 normal(Vector4 vertex)
        {
            Vector3 n = new Vector3(0, 0, 0);
            Dictionary<Vector4, HalfEdge> d = halfEdges[vertex];

            foreach (KeyValuePair<Vector4, HalfEdge> kv in d) {
                Face f = kv.Value.face;
                n += f.normal;
            }

            return Vector3.Normalize(n);
        }

        private HalfEdge existHalfEdge(Vector4 origin, Vector4 dest)
        {
            // check if halfedge exists in O(1)
            if(halfEdges.ContainsKey(origin) && halfEdges[origin].ContainsKey(dest))
                return halfEdges[origin][dest];
            return null;
        }

    }

    public class Face
    {
        public Vector3 normal;
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
