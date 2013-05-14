using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.IO;

namespace Utilities
{
    class Obj
    {
        List<Vector4> vertices = new List<Vector4>();
        List<Vector4> normals = new List<Vector4>();
        List<Vector4> textures = new List<Vector4>();

        public Obj(string path)
        {
            FileStream fs = File.Open(path, FileMode.Open);

            StreamReader reader = new StreamReader(fs);
            
            string line = reader.ReadLine();
            
            line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");

            string[] values = line.Split(' ');

            // Read vertices
            while (values[0] == "v")
            {
                Vector4 v = new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1f);
                vertices.Add(v);

                line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read texture
            while (values[0] == "vt")
            {
                textures.Add(new Vector4(float.Parse(values[1]), float.Parse(values[1]), 0f, 1f));

                line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read normals
            while (values[0] == "vn")
            {
                normals.Add(new Vector4(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]), 1f));

                line = line.Trim(' ').Replace('.', ',').Replace("  ", " ");
                values = line.Split(' ');
            }

            // Read faces
            while (values[0] == "f")
            {
                //polynet.addFace();
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
