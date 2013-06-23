using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Utilities
{
    public class Materials : Dictionary<string, Material>
    {
        public static Materials Singleton = new Materials();
        public void AddMaterial(Material mat)
        {
            if (Singleton.ContainsKey(mat.Name))
                return;

            Singleton.Add(mat.Name, mat);
        }
    }

    public class Material
    {
        public string Name { get; set; }
        public Color Ambient { get; set; }
        public Color Diffuse { get; set; }
        public Color Specular { get; set; }
        public Color Emission { get; set; }
        public float Shininess { get; set; }

        public Texture Texture { get; set; }
    }
}
