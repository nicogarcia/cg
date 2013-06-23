using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using OpenTK;
using Utilities.Shapes;
using System.Threading;
using System.Globalization;

namespace Utilities
{
    public class SharpObjLoader
    {
        public List<Vertex> vertices = new List<Vertex>();

        private System.Drawing.Color ReadMaterialColor(string line, float alpha)
        {
            string[] lineParts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (lineParts.Length >= 4)
            {
                // Convert float a,r,g,b values to byte values.  Make sure they fall in 0-255 range.
                int a = Convert.ToInt32(255 * alpha);
                if (a < 0) a = 0; if (a > 255) a = 255;
                int r = Convert.ToInt32(255 * Convert.ToSingle(lineParts[1]));
                if (r < 0) r = 0; if (r > 255) r = 255;
                int g = Convert.ToInt32(255 * Convert.ToSingle(lineParts[2]));
                if (g < 0) g = 0; if (g > 255) g = 255;
                int b = Convert.ToInt32(255 * Convert.ToSingle(lineParts[3]));
                if (b < 0) b = 0; if (b > 255) b = 255;
                return System.Drawing.Color.FromArgb(a, r, g, b);
            }
            else
                return System.Drawing.Color.White;
        }

        private void SetAlphaForMaterial(Material material, float alpha)
        {
            int a = Convert.ToInt32(255 * alpha);
            material.Ambient = System.Drawing.Color.FromArgb(a, material.Ambient);
            material.Diffuse = System.Drawing.Color.FromArgb(a, material.Diffuse);
            material.Specular = System.Drawing.Color.FromArgb(a, material.Specular);
            material.Emission = System.Drawing.Color.FromArgb(a, material.Emission);
        }

        private string ReadMaterialValue(string line)
        {
            //  The material is everything after the first space.
            int spacePos = line.IndexOf(' ');
            if (spacePos == -1 || (spacePos + 1) >= line.Length)
                return null;

            //  Return the material path.
            return line.Substring(spacePos + 1);
        }
        
        private void LoadMaterials(string path)
        {

            //  Create a stream reader.
            using (StreamReader reader = new StreamReader(path))
            {
                Material mtl = null;
                float alpha = 1;

                //  Read line by line.
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    //  Skip any comments (lines that start with '#').
                    if (line.StartsWith("#"))
                        continue;

                    // newmatl indicates start of material definition.
                    if (line.StartsWith("newmtl"))
                    {
                        // Add new material to scene's assets.
                        mtl = new Material();
                        // Name of material is on same line, immediately follows newmatl.
                        mtl.Name = ReadMaterialValue(line);
                        Materials.Singleton.AddMaterial(mtl);


                        // Reset assumed alpha.
                        alpha = 1;
                    }

                    // Read properties of material.
                    if (mtl != null)
                    {
                        if (line.StartsWith("Ka"))
                            mtl.Ambient = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Kd"))
                            mtl.Diffuse = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Ks"))
                            mtl.Specular = ReadMaterialColor(line, alpha);
                        else if (line.StartsWith("Ns"))
                            mtl.Shininess = Convert.ToSingle(ReadMaterialValue(line));
                        else if (line.StartsWith("map_Ka") ||
                            line.StartsWith("map_Kd") ||
                            line.StartsWith("map_Ks"))
                        {
                            // Get texture map.                    		
                            string textureFile = ReadMaterialValue(line);

                            // Check for existing textures.  Create if does not exist.
                            Texture theTexture = null;
                            var existingTextures = Textures.Singleton.Values.Where(t => t is Texture && t.filename == textureFile);
                            if (existingTextures.Count() >= 1)
                                theTexture = existingTextures.FirstOrDefault() as Texture;
                            else
                            {
                                //  Does the texture file exist?
                                if (File.Exists(textureFile) == false)
                                {
                                    //  It doesn't, assume its in the same location
                                    //  as the obj file.
                                    textureFile = Path.Combine(Path.GetDirectoryName(path),
                                        Path.GetFileName(textureFile));
                                }
                                
                                // Create/load texture.
                                theTexture = new Texture(textureFile);
                                Textures.AddTexture(theTexture);
                                //theTexture.Create(scene.OpenGL, textureFile);
                            }

                            // Set texture for material.
                            mtl.Texture = theTexture;
                        }
                        else if (line.StartsWith("d") || line.StartsWith("Tr"))
                        {
                            alpha = Convert.ToSingle(ReadMaterialValue(line));
                            SetAlphaForMaterial(mtl, alpha);
                        }
                        // TODO: Handle illumination mode (illum)                    	                    
                    }
                }

            }
        }

        public SharpObjLoader(string path)
        {
            string errors = "";
            List<Vector4> positions = new List<Vector4>();
            List<Vector4> textures = new List<Vector4>();
            List<Vector4> normals = new List<Vector4>();

            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            char[] split = new char[] { ' ' };
            
            string mtlName = null;

            //  Create a stream reader.
            using (StreamReader reader = new StreamReader(path))
            {
                //  Read line by line.
                string line = null;
                while ((line = reader.ReadLine()) != null)
                {
                    //line = line.Replace('.', ',');
                    //  Skip any comments (lines that start with '#').
                    if (line.StartsWith("#"))
                        continue;

                    //  Do we have a texture coordinate?
                    if (line.StartsWith("vt"))
                    {
                        //  Get the texture coord strings.
                        string[] values = line.Substring(3).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Parse texture coordinates.
                        float u = float.Parse(values[0]);
                        float v = float.Parse(values[1]);

                        //  Add the texture coordinate.
                        //polygon.UVs.Add(new UV(u, v));
                        textures.Add(new Vector4(u, v, 0f, 1f));

                        continue;
                    }

                    //  Do we have a normal coordinate?
                    if (line.StartsWith("vn"))
                    {
                        //  Get the normal coord strings.
                        string[] values = line.Substring(3).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Parse normal coordinates.
                        float x = float.Parse(values[0]);
                        float y = float.Parse(values[1]);
                        float z = float.Parse(values[2]);

                        //  Add the normal.
                        //polygon.Normals.Add(new Vertex(x, y, z));
                        normals.Add(new Vector4(x, y, z, 1f));

                        continue;
                    }

                    //  Do we have a vertex?
                    if (line.StartsWith("v"))
                    {
                        //  Get the vertex coord strings.
                        string[] values = line.Substring(2).Split(split, StringSplitOptions.RemoveEmptyEntries);

                        //  Parse vertex coordinates.
                        float x = float.Parse(values[0]);
                        float y = float.Parse(values[1]);
                        float z = float.Parse(values[2]);

                        //   Add the vertices.
                        //polygon.Vertices.Add(new Vertex(x, y, z));
                        positions.Add(new Vector4(x, y, z, 1f));

                        continue;
                    }

                    //  Do we have a face?
                    if (line.StartsWith("f"))
                    {
                        //Face face = new Face();

                        //if (!String.IsNullOrWhiteSpace(mtlName))
                        //    face.Material = scene.Assets.Where(t => t.Name == mtlName).FirstOrDefault() as Material;

                        //  Get the face indices
                        string[] indices = line.Substring(2).Split(split,
                            StringSplitOptions.RemoveEmptyEntries);

                        //  Add each index.
                        for (int i = 0; i < indices.Length - 2; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                //  Split the parts.
                                string[] parts = indices[j + i].Split(new char[] { '/' }, StringSplitOptions.None);

                                //  Add each part.
                                int position_index = (parts.Length > 0 && parts[0].Length > 0) ? int.Parse(parts[0]) - 1 : -1;
                                int texture_index = (parts.Length > 1 && parts[1].Length > 0) ? int.Parse(parts[1]) - 1 : -1;
                                int normal_index = (parts.Length > 2 && parts[2].Length > 0) ? int.Parse(parts[2]) - 1 : -1;

                                Vertex v = new Vertex(positions[position_index]);
                                if (texture_index != -1)
                                    v.texture = textures[texture_index];
                                if (normal_index != -1)
                                    v.normal = normals[normal_index];

                                vertices.Add(v);
                            }
                        }

                        //  Add the face.
                        //polygon.Faces.Add(face);

                        continue;
                    }
                    
                    if (line.StartsWith("mtllib"))
                    {
                        // Set current directory in case a relative path to material file is used.
                        //Environment.CurrentDirectory = Path.GetDirectoryName(path);

                        // Load materials file.
                        string mtlPath = ReadMaterialValue(line);
                        LoadMaterials("../../" + mtlPath);
                        continue;
                    }

                    if (line.StartsWith("usemtl"))
                    {
                        mtlName = ReadMaterialValue(line);
                        continue;
                    }
                    
                    errors += "Unsupported keyword " + line + "\n";

                }
            }

            // Print Errors in different thread
            Thread t = new Thread(new ThreadStart(delegate(){
                Console.WriteLine(errors);
            }));
            t.Start();
            
            //scene.SceneContainer.AddChild(polygon);

            //return scene;
        }

    }
}
