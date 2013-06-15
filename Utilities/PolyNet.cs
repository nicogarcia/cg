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
        public Dict<Vertex,HalfEdge> halfEdges;

        public PolyNet()
        {
            faces = new List<Face>();
            halfEdges = new Dict<Vertex, HalfEdge>();
        }

        public void addFace(Vertex[] vertices)
        {
            HalfEdge[] currentHalfEdges = new HalfEdge[vertices.Length];

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
                    currentHalfEdges[curr] = new HalfEdge(vertices[curr], vertices[next_he]);
                    HalfEdge twin = new HalfEdge(vertices[next_he],vertices[curr]);

                    currentHalfEdges[curr].twin = twin;
                    twin.twin = currentHalfEdges[curr];
                    // add halfedge to polynet halfedges dictionary
                    this.halfEdges.Add(vertices[curr], vertices[next_he], currentHalfEdges[curr]);
                    this.halfEdges.Add(vertices[next_he], vertices[curr], twin);
                }
                else
                {
                    currentHalfEdges[curr] = exist;
                }
            }

            // create face with some halfedge
            Face face = new Face(currentHalfEdges[0]);
            // calculate face's normal
            face.normal = faceNormal(vertices);
            // add face to faces list
            faces.Add(face);

            // link halfedges with next and prev
            for(int i = 0; i < currentHalfEdges.Length; i++){
                int next = (i + 1) % currentHalfEdges.Length;
                int prev = (currentHalfEdges.Length + i - 1) % currentHalfEdges.Length;

                currentHalfEdges[i].next = currentHalfEdges[next];
                currentHalfEdges[i].prev = currentHalfEdges[prev];

                // link face to halfedge
                currentHalfEdges[i].face = face;
            }
        }

        private Vector3 faceNormal(Vertex[] vertices)
        {
            Vector3 edge1, edge2;

            edge1 = new Vector3(vertices[1].position - vertices[0].position);
            edge2 = new Vector3(vertices[2].position - vertices[1].position);

            // get face's normal vector with cross product
            Vector3 ret = Vector3.Cross(edge1, edge2);
            return ret;
        }

        // gets vertex normal as an average of faces normals
        public Vector4 normal(Vertex vertex)
        {
            Vector3 n = new Vector3(0, 0, 0);
            Dictionary<Vertex, HalfEdge> d = halfEdges[vertex];

            foreach (KeyValuePair<Vertex, HalfEdge> kv in d) {
                Face f = kv.Value.face;
                n += f.normal;
            }

            return Vector4.Normalize(new Vector4(n));
        }

        // gets all vertex normals
        public List<Vector4> normals(Vertex vertex)
        {
            Dictionary<Vertex, HalfEdge> d = halfEdges[vertex];
            List<Vector4> normals = new List<Vector4>();

            foreach (KeyValuePair<Vertex, HalfEdge> kv in d)
            {
                Face f = kv.Value.face;
                 normals.Add(new Vector4(f.normal));
            }

            return normals;
        }

        private HalfEdge existHalfEdge(Vertex origin, Vertex dest)
        {
            // check if halfedge exists in 
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

        public Vertex[] vertices()
        {
            List<Vertex> list = new List<Vertex>();
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
        public Vertex origin;
        public HalfEdge twin;
        public HalfEdge prev, next;
        public Face face;

        public HalfEdge(Vertex origin, Vertex dest)
        {
            this.origin = origin;
            /*
            twin = new HalfEdge();
            twin.origin = dest;
            twin.twin = this;*/

        }

        public HalfEdge()
        {
        }
    }
}
