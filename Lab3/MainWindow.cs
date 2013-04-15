using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Utilities.Shaders;
using System.IO;

namespace Lab3
{
    public partial class MainWindow : Form
    {
        int VAO_ID;
        int VBO_ID;
        int projection_location;
        int model_view_location;
        ProgramObject program;
        Matrix4 projMatrix;
        Matrix4 zoomMatrix;
        bool selectA = true;

        Vector4[][] vertices;
        Vector4[] single_array;
        float[] bounds = new float[4];

        // Figure aspect ratio
        float aspect_x, aspect_y;

        // Selection aspect ratio
        float selection_x = 0.15f, selection_y = 0.1f;

        // Left viewport
        Rectangle left_viewport;

        bool picture_loaded = false;

        public MainWindow()
        {
            InitializeComponent();
            aToolStripMenuItem.Enabled = false;

        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            program = new ProgramObject(
            new VertexShader(Shaders.VERTEX_TRANSF_SHADER), new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));
            GL.ClearColor(Color.Black);

            GL.GenVertexArrays(1, out VAO_ID);
            GL.GenBuffers(1, out VBO_ID);

            projection_location = GL.GetUniformLocation(program.program_handle, "projectionMatrix");
            model_view_location = GL.GetUniformLocation(program.program_handle, "modelView");


            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);
        }


        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (vertices != null)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(program.program_handle);

                GL.BindVertexArray(VAO_ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);

                GL.UniformMatrix4(projection_location, false, ref projMatrix);
                GL.UniformMatrix4(model_view_location, false, ref Matrix4.Identity);

                // Calculate resize ratios for resizing
                int ratioW;
                if (selectA)
                    ratioW = (int)(glControl1.Width / aspect_x);
                else
                    ratioW = (int)(glControl1.Width / 2 / aspect_x);
                int ratioH = (int)(glControl1.Height / aspect_y);

                // smaller ratio will ensure that the image fits in the view
                int ratio = ratioW < ratioH ? ratioW : ratioH;
                
                if (selectA)
                    left_viewport = new Rectangle(
                        (int)(glControl1.Width - aspect_x * ratio) / 2,
                        (int)(glControl1.Height - aspect_y * ratio) / 2,
                        (int)(aspect_x * ratio),
                        (int)(aspect_y * ratio));

                else left_viewport = new Rectangle(
                       (int)(glControl1.Width / 2 - aspect_x * ratio) / 2,
                       (int)(glControl1.Height - aspect_y * ratio) / 2,
                       (int)(aspect_x * ratio),
                       (int)(aspect_y * ratio));
                GL.Viewport(left_viewport);

                int current = 0;
                for (int i = 0; i < vertices.Length; i++)
                {
                    GL.DrawArrays(BeginMode.LineStrip, current, vertices[i].Length);
                    current += vertices[i].Length;
                }

                // Calculate resize ratios for resizing
                if (!selectA)
                {
                    int sel_ratioW = (int)(glControl1.Width / 2 / selection_x);
                    int sel_ratioH = (int)(glControl1.Height / selection_y);

                    // smaller ratio will ensure that the image fits in the view
                    int sel_ratio = sel_ratioW < sel_ratioH ? sel_ratioW : sel_ratioH;

                    GL.UniformMatrix4(projection_location, false, ref zoomMatrix);
                    GL.UniformMatrix4(model_view_location, false, ref Matrix4.Identity);

                    GL.Viewport(
                        (int)(glControl1.Width / 2 - selection_x * sel_ratio) / 2 + glControl1.Width / 2,
                        (int)(glControl1.Height - selection_y * sel_ratio) / 2,
                        (int)(selection_x * sel_ratio),
                        (int)(selection_y * sel_ratio));

                    current = 0;
                    for (int i = 0; i < vertices.Length; i++)
                    {
                        GL.DrawArrays(BeginMode.LineStrip, current, vertices[i].Length);
                        current += vertices[i].Length;
                    }
                }

                GL.UseProgram(0);

                glControl1.SwapBuffers();
            }
        }

        private Vector4[] createSingleVerticesArray()
        {
            int count = 0;
            for (int i = 0; i < vertices.Length; i++)
                count += vertices[i].Length;

            Vector4[] single = new Vector4[count];

            int current = 0;
            for (int i = 0; i < vertices.Length; i++)
            {
                for (int j = 0; j < vertices[i].Length; j++)
                {
                    single[current++] = vertices[i][j];
                }
            }

            return single;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();

            if (result != DialogResult.OK)
                return;

            label1.Text = folderBrowserDialog1.SelectedPath;

            DirectoryInfo dinfo = new DirectoryInfo(folderBrowserDialog1.SelectedPath);
            FileInfo[] Files = dinfo.GetFiles("*.grs");

            listBox1.Items.Clear();
            foreach (FileInfo file in Files)
            {
                listBox1.Items.Add(file.Name);
            }

        }

        private void glControl_Click(object sender, EventArgs e)
        {
            if (!selectA)
            {
                // Selection box offsets
                float delta_x = 0.1f;
                float delta_y = 0.1f;

                MouseEventArgs mea = (MouseEventArgs)e;
                float screen_x = mea.X;
                float screen_y = glControl1.Height - mea.Y;

                Vector3 mouse_position = new Vector3(screen_x, screen_y, 0f);

                Vector4 world_position = new Vector4(
                        vector_unproject(
                            mouse_position,
                            Matrix4.Identity,
                            projMatrix,
                            new Rectangle(0, 0, glControl1.Width / 2, glControl1.Height)
                         ),
                         1f);

                zoomMatrix = Matrix4.CreateOrthographicOffCenter(
                    world_position.X - delta_x,
                    world_position.X + delta_x,
                    world_position.Y - delta_y,
                    world_position.Y + delta_y,
                    -1f, 1f);

                glControl1.Invalidate();
            }
        }

        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (!selectA)
            {
                if (!picture_loaded)
                    return;

                MouseEventArgs mea = (MouseEventArgs)e;
                float screen_x = mea.X;
                float screen_y = glControl1.Height - mea.Y;

                Vector3 mouse_position = new Vector3(screen_x, screen_y, 0f);

                Vector4 world_position = new Vector4(
                        vector_unproject(
                            mouse_position,
                            Matrix4.Identity,
                            projMatrix,
                            left_viewport),
                            1f
                    );

                zoomMatrix = Matrix4.CreateOrthographicOffCenter(
                    world_position.X - selection_x,
                    world_position.X + selection_x,
                    world_position.Y - selection_y,
                    world_position.Y + selection_y,
                    -1f, 1f);

                glControl1.Invalidate();
            }
        }

        private void list_SelectedChanged(object sender, EventArgs e)
        {
            picture_loaded = false;

            string[] lines = System.IO.File.ReadAllLines(label1.Text + "\\" + listBox1.SelectedItem.ToString());

            // i stores the reading cursor
            int i = 0;
            while (lines[i++][0] != '*') { }

            // At the line after '**************'

            // Replace double spaces by only one
            lines[i] = lines[i].Replace("  ", " ");
            // Remove starting space and replace dot by comma
            lines[i] = lines[i].Replace('.', ',');

            // Read the bounds of the image
            string[] extent = lines[i++].Split(' ');

            float.TryParse(extent[0], out bounds[0]);
            float.TryParse(extent[1], out bounds[1]);
            float.TryParse(extent[2], out bounds[2]);
            float.TryParse(extent[3], out bounds[3]);

            // Read the number of polylines in the file
            int num_of_polylines;
            int.TryParse(lines[i++], out num_of_polylines);
            vertices = new Vector4[num_of_polylines][];

            // For each polyline
            for (int j = 0; j < num_of_polylines; j++)
            {
                // Read the number of vertices
                int num_of_vertices;
                int.TryParse(lines[i++], out num_of_vertices);

                vertices[j] = new Vector4[num_of_vertices];

                // For each vertex
                for (int k = 0; k < num_of_vertices; k++)
                {
                    // Replace double spaces by ony one
                    lines[i] = lines[i].TrimStart(' ').Replace("  ", " ");
                    // Remove starting space and replace dot by comma
                    lines[i] = lines[i].Replace('.', ',');

                    float x, y;
                    string[] parts = lines[i++].Split(' ');

                    float.TryParse(parts[0], out x);
                    float.TryParse(parts[1], out y);

                    vertices[j][k] = new Vector4(x, y, 0f, 1f);
                }
            }

            // Initialize projection and zoom with picture extents
            projMatrix = Matrix4.CreateOrthographicOffCenter(bounds[0], bounds[2], bounds[3], bounds[1], -1f, 1f);
            zoomMatrix = Matrix4.Identity;

            // Define aspect ratio
            aspect_x = bounds[2] - bounds[0];
            aspect_y = bounds[1] - bounds[3];

            single_array = createSingleVerticesArray();

            GL.BindVertexArray(VAO_ID);
            GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(single_array.Length * Vector4.SizeInBytes),
                            single_array, BufferUsageHint.StaticDraw);

            glControl1.Invalidate();

            picture_loaded = true;
        }


        Vector3 vector_unproject(Vector3 win, Matrix4 mViewMat, Matrix4 projMat, Rectangle viewport)
        {
            Vector3 resul = Vector3.Zero;
            Vector4 _in = Vector4.Zero;
            Vector4 _out = Vector4.Zero;
            Matrix4 oneMatrix;
            //Combinamos las dos matrices y las invertimos.
            oneMatrix = Matrix4.Mult(mViewMat, projMat);
            oneMatrix.Invert();

            _in.X = win.X;
            _in.Y = win.Y;
            _in.Z = win.Z;
            _in.W = 1.0f;
            //Map x and y from window coordinates.
            _in.X = (_in.X - viewport.X) / viewport.Width;
            _in.Y = (_in.Y - viewport.Y) / viewport.Height;
            //Map to range -1 to 1.
            _in.X = _in.X * 2 - 1;
            _in.Y = _in.Y * 2 - 1;
            _in.Z = _in.Z * 2 - 1;

            //Antitransformamos.
            _out = Vector4.Transform(_in, oneMatrix);
            if ((_out.W > float.Epsilon) || (_out.W < -float.Epsilon))
            {
                _out.X = _out.X / _out.W;
                _out.Y = _out.Y / _out.W;
                _out.Z = _out.Z / _out.W;
            }
            else
            {
                throw new Exception("UnProject: No pudo antitransformar.");
            }

            resul.X = _out.X;
            resul.Y = _out.Y;
            resul.Z = _out.Z;
            return resul;
        }

        private void aToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectA = true;
            aToolStripMenuItem.Enabled = false;
            bToolStripMenuItem.Enabled = true;
        }

        private void bToolStripMenuItem_Click(object sender, EventArgs e)
        {
            selectA = false;
            aToolStripMenuItem.Enabled = true;
            bToolStripMenuItem.Enabled = false;
        }

    }
}

