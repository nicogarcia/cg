using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using System.ComponentModel;
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
        public Vector3 Ambient { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Specular { get; set; }
        public Vector3 Emission { get; set; }
        public float Shininess { get; set; }

        [DefaultValue(1f)]
        public float Alpha { get; set; }

        public Texture Texture { get; set; }

        public Material(string name, Texture texture)
        {
            Name = name;
            Texture = texture;
            Textures.AddTexture(texture);
        }

        public Material()
        {
            // TODO: Complete member initialization
        }

    }
}
