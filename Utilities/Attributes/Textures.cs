using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace Utilities
{
    public class Textures : Dictionary<string, Texture>
    {
        public static Textures Singleton = new Textures();

        public static void AddTexture(Texture tex)
        {
            if (Singleton.ContainsKey(tex.filename))
                return;

            if (String.IsNullOrEmpty(tex.filename))
                throw new ArgumentException(tex.filename);

            GL.BindTexture(TextureTarget.Texture2D, tex.id);

            Bitmap bmp = new Bitmap(tex.filename);
            BitmapData bmp_data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bmp_data.Width, bmp_data.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bmp_data.Scan0);

            bmp.UnlockBits(bmp_data);

            // We haven't uploaded mipmaps, so disable mipmapping (otherwise the texture will not appear).
            // On newer video cards, we can use GL.GenerateMipmaps() or GL.Ext.GenerateMipmaps() to create
            // mipmaps automatically. In that case, use TextureMinFilter.LinearMipmapLinear to enable them.
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            GL.BindTexture(TextureTarget.Texture2D, 0);

            Singleton.Add(tex.filename, tex);
        }

        public static void LoadTexture(string filename)
        {
            GL.BindTexture(TextureTarget.Texture2D, Singleton[filename].id);
        }

    }

    public class Texture
    {
        public string filename { get; set; }
        public int id { get; set; }

        public Texture(string filename)
        {
            this.filename = filename;
            id = GL.GenTexture();
        }

    }
}
