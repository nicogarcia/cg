using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Utilities
{
    class ObjLoader
    {
        public List<Vertex> vertices = new List<Vertex>();

        public ObjLoader(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open);

            StreamReader reader = new StreamReader(fs);
            
            string line = reader.ReadLine();
            
            line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");

            string[] values = line.Split(' ');

            List<Vector4> positions = new List<Vector4>();
            List<Vector4> textures = new List<Vector4>();
            List<Vector4> normals = new List<Vector4>();

            // Read vertices
            while (values[0] == "v")
            {
                positions.Add( new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1f));

                line = reader.ReadLine().Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read texture
            while (values[0] == "vt")
            {
                textures.Add( new Vector4(float.Parse(values[1]), float.Parse(values[2]), 0f, 1f));

                line = reader.ReadLine().Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read normals
            while (values[0] == "vn")
            {
                normals.Add( new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1f));

                line = reader.ReadLine().Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read faces
            while (values[0] == "f")
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] v_3 = values[i + 1].Split('/');
                    Vertex v = new Vertex(positions[int.Parse(v_3[0]) - 1]);
                    v.texture = textures[int.Parse(v_3[1]) - 1];
                    v.normal = normals[int.Parse(v_3[2]) - 1];

                    vertices.Add(v);
                }

                line = reader.ReadLine();
                if (line == null)
                    return;
                line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

        }
    }

    class ObjFace
    {
        Vector4[] vertex = new Vector4[3];
        Vector4[] normal= new Vector4[3];
        Vector4[] texture= new Vector4[3];

        public Vector4[] getRaw(){
            return vertex.Concat(normal).Concat(texture).ToArray();
        }
    }
}
