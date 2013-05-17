using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace Utilities
{
    public struct LightInfo
    {
        Vector4 position;
        Vector3 intensity;
    };

    public struct MaterialInfo
    {
        Vector3 ambient_reflectivity;
        Vector3 diffuse_reflectivity;
        Vector3 specular_reflectivity;
        float Shininess;
    };
}
