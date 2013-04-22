using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities;
using Utilities.Shaders;

namespace Lab5
{
    public partial class MainWindow : Form
    {
        int VAO_ID;
        int VBO_ID;
        int projection_location;
        int model_view_location;
        ProgramObject program;

        Camera cam;

        Matrix4 rotx = Matrix4.Identity;
        Matrix4 roty = Matrix4.Identity;
        Matrix4 rotz = Matrix4.Identity;

        /*Matrix4 projMatrix = Matrix4.CreateRotationX((float)Math.PI / 4f) * Matrix4.CreateRotationY((float)Math.PI / 4f)
            * Matrix4.CreateTranslation(new Vector3(0, 1f, 1.5f)) *Matrix4.Scale(0.5f) ;*/
        Matrix4 projMatrix = Matrix4.Identity;
        Matrix4 zoomMatrix = Matrix4.Identity;

        Vector4[] toDraw;
        int[] first;
        int[] count;

        Vector4[] normals;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            program = new ProgramObject(
            new VertexShader(Shaders.VERTEX_TRANSF_SHADER), new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));
            GL.ClearColor(Color.Yellow);

            GL.GenVertexArrays(1, out VAO_ID);
            GL.GenBuffers(1, out VBO_ID);

            projection_location = GL.GetUniformLocation(program.program_handle, "projectionMatrix");
            model_view_location = GL.GetUniformLocation(program.program_handle, "modelView");

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

            Foot foot = new Foot(2f, 20, new Vector4[]{
                new Vector4(0.5f, -0.5f, 0, 1f),
                new Vector4(-0.5f, -0.5f, 0, 1f),
                new Vector4(-0.5f, 0.5f, 0, 1f),
                new Vector4(0.5f, 0.5f, 0, 1f),
            });

            float x = 1.0f;
            float y = 1.0f;
            float z = 2f;
            float aux = 0.15f;

            Vector4[] bottom = new Vector4[]{new Vector4(-x, -y + aux, z, 1.0f),
               new Vector4(-x + aux, -y, z, 1.0f),
               new Vector4(x - aux, -y, z, 1.0f),
               new Vector4(x, -y + aux, z, 1.0f),
               new Vector4(x, y - aux, z, 1.0f),
               new Vector4(x - aux, y, z, 1.0f),
               new Vector4(-x + aux, y, z, 1.0f),
               new Vector4(-x, y - aux, z, 1.0f)
            };

            Cover cover = new Cover(bottom);

            int numberElements = foot.polynet.faces.Count * 4 + (cover.polynet.faces.Count - 2) * 4 + 16;
            //int numberElements = foot.polynet.faces.Count;

            first = new int[numberElements];
            count = new int[numberElements];
            toDraw = new Vector4[numberElements * 4];


            normals = new Vector4[168];
            Vector4[] vert = foot.polynet.halfEdges.Keys.ToArray();
            int cur = 0;
            for (int i = 0; i < normals.Length - 1; i += 2, cur += 2)
            {
                normals[i] = vert[i / 2];
                normals[i + 1] = normals[i] + new Vector4(foot.polynet.normal(normals[i]), 0);
            }

            int cursor = 0;
            int fcur = 0;
            foreach (Face face in foot.polynet.faces)
            {
                // Add face's vertices to "toDraw" array
                Vector4[] array = face.vertices();

                first[fcur] = cursor;
                count[fcur++] = array.Length;
                for (int i = 0; i < array.Length; i++)
                {
                    toDraw[cursor++] = array[i];
                }

            }

            foreach (Face face in cover.polynet.faces)
            {
                Vector4[] array = face.vertices();

                first[fcur] = cursor;
                count[fcur++] = array.Length;
                for (int i = 0; i < array.Length; i++)
                {
                    toDraw[cursor++] = array[i];
                }
            }

            cam = new Camera(new Spherical(4f, (float)-Math.PI / 4f, (float)Math.PI / 6f));

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(toDraw.Length * Vector4.SizeInBytes),
                            toDraw, BufferUsageHint.StaticDraw);

            glControl1.Invalidate();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (toDraw != null)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(program.program_handle);

                GL.BindVertexArray(VAO_ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

                int aspect_x = 1, aspect_y = 1;

                // Calculate resize ratios for resizing
                int ratioW = (int)(glControl1.Width / aspect_x);
                int ratioH = (int)(glControl1.Height / aspect_y);

                // smaller ratio will ensure that the image fits in the view
                int ratio = ratioW < ratioH ? ratioW : ratioH;

                Rectangle left_viewport = new Rectangle(
                        (int)(glControl1.Width - aspect_x * ratio) / 2,
                        (int)(glControl1.Height - aspect_y * ratio) / 2,
                        (int)(aspect_x * ratio),
                        (int)(aspect_y * ratio));

                GL.Viewport(left_viewport);

                // apply rotation 
                projMatrix = rotx * roty * rotz;
                projMatrix *= Matrix4.CreatePerspectiveFieldOfView(2f, 1f, 0.1f, 100f);
                //projMatrix *= Matrix4.CreateOrthographicOffCenter(-5f, 5f, -5f, 5f, -5f, 1000f);

                zoomMatrix = cam.lookAt();

                GL.UniformMatrix4(projection_location, false, ref projMatrix);
                GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);

                // Remooooveee!!!
                GL.BindVertexArray(VAO_ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(toDraw.Length * Vector4.SizeInBytes),
                                toDraw, BufferUsageHint.StaticDraw);

                for (int i = 0; i < toDraw.Length / 4; i++)
                {
                    GL.MultiDrawArrays(BeginMode.LineLoop, first, count, count.Length);
                }


                GL.BindVertexArray(VAO_ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(normals.Length * Vector4.SizeInBytes),
                                normals, BufferUsageHint.StaticDraw);
                for (int i = 0; i < 1; i++)
                {
                    GL.DrawArrays(BeginMode.Lines, 0, normals.Length);
                }

                GL.UseProgram(0);

                glControl1.SwapBuffers();
            }
        }

        private void sliderx_scroll(object sender, EventArgs e)
        {
            TrackBar track = (TrackBar)sender;
            rotx = Matrix4.CreateRotationX(track.Value * 2f * (float)Math.PI / (float)track.Maximum);
            glControl1.Invalidate();
        }
        private void slidery_scroll(object sender, EventArgs e)
        {
            TrackBar track = (TrackBar)sender;
            roty = Matrix4.CreateRotationY(track.Value * 2 * (float)Math.PI / track.Maximum);
            glControl1.Invalidate();
        }
        private void sliderz_scroll(object sender, EventArgs e)
        {
            TrackBar track = (TrackBar)sender;
            rotz = Matrix4.CreateRotationZ(track.Value * 2 * (float)Math.PI / track.Maximum);
            glControl1.Invalidate();
        }

        private void glControl_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '8':
                    cam.position.growTheta();
                    break;
                case '5':
                    cam.position.shrinkTheta();
                    break;
                case '6':
                    cam.position.growPhi();
                    break;
                case '4':
                    cam.position.shrinkPhi();
                    break;
                case '-':
                    cam.position.growRadio();
                    break;
                case '+':
                    cam.position.shrinkRadio();
                    break;
            }

            glControl1.Invalidate();

        }

        private void glControl_scroll(object sender, MouseEventArgs e)
        {
            if (e.Delta < 0)
                cam.position.growRadio();
            else
                cam.position.shrinkRadio();

            glControl1.Invalidate();
        }

        private void resize(object sender, EventArgs e)
        {

        }
    }
}
/*
// Generate cube

// Vertices
Vector4[] vertices = new Vector4[]{
    new Vector4(0f,0f,0f,1f),
    new Vector4(1f,0f,0f,1f),
    new Vector4(1f,0f,1f,1f),
    new Vector4(0f,0f,1f,1f),
    new Vector4(0f,1f,1f,1f),
    new Vector4(1f,1f,1f,1f),
    new Vector4(1f,1f,0f,1f),
    new Vector4(0f,1f,0f,1f)
};

polynet.addFace(new Vector4[]{
    vertices[0],
    vertices[1],
    vertices[2],
    vertices[3],
});

polynet.addFace(new Vector4[]{
    vertices[1],
    vertices[6],
    vertices[5],
    vertices[2],
});
polynet.addFace(new Vector4[]{
    vertices[6],
    vertices[7],
    vertices[4],
    vertices[5],
});
polynet.addFace(new Vector4[]{
    vertices[0],
    vertices[3],
    vertices[4],
    vertices[7],
});
polynet.addFace(new Vector4[]{
    vertices[3],
    vertices[2],
    vertices[5],
    vertices[4],
});
polynet.addFace(new Vector4[]{
    vertices[0],
    vertices[7],
    vertices[6],
    vertices[1],
});*/