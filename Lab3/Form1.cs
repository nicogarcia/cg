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

namespace Lab3
{
    public partial class Form1 : Form
    {
        int VAO_ID;
        int VBO_ID;
        int projection_location;
        int model_view_location;
        Vector4[][] vertices;
        float[] bounds = new float[4];
        float aspect_x, aspect_y;
        ProgramObject program;
        Matrix4 projMatrix;
        Matrix4 zoomMatrix;
        

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            program =  new ProgramObject(
            new VertexShader(Shaders.VERTEX_TRANSF_SHADER), new FragmentShader(Shaders.DEFAULT_FRAGMENT_SHADER));
            GL.ClearColor(Color.Yellow);

            GL.GenVertexArrays(1, out VAO_ID);
            GL.GenBuffers(1, out VBO_ID);

            projection_location = GL.GetUniformLocation(program.program_handle, "projectionMatrix");
            model_view_location = GL.GetUniformLocation(program.program_handle, "modelView");

        }
        

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            if (vertices != null)
            {
                GL.Clear(ClearBufferMask.ColorBufferBit);

                GL.UseProgram(program.program_handle);

                GL.BindVertexArray(VAO_ID);
                GL.BindBuffer(BufferTarget.ArrayBuffer, VBO_ID);
                
                // Calculate resize ratios for resizing
                int ratioW = (int) (glControl1.Width / 2 / aspect_x);
                int ratioH = (int) (glControl1.Height / aspect_y);

                // smaller ratio will ensure that the image fits in the view
                int ratio = ratioW < ratioH ? ratioW : ratioH;

                foreach (Vector4[] polylines in vertices)
                {
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(polylines.Length * Vector4.SizeInBytes),
                                    polylines, BufferUsageHint.StaticDraw);

                    GL.UniformMatrix4(projection_location, false, ref projMatrix);
                    GL.UniformMatrix4(model_view_location, false, ref Matrix4.Identity);
                    
                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

                    GL.Viewport((int) (glControl1.Width / 2 - aspect_x * ratio) / 2, (int) (glControl1.Height - aspect_y * ratio) / 2,
                    (int) (aspect_x * ratio), (int) (aspect_y * ratio));
                    
                    GL.DrawArrays(BeginMode.LineStrip, 0, polylines.Length);
                }
                
                foreach (Vector4[] polylines in vertices)
                {
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(polylines.Length * Vector4.SizeInBytes),
                                    polylines, BufferUsageHint.StaticDraw);

                    Matrix4 zoom = Matrix4.Scale(1f) * zoomMatrix;

                    GL.UniformMatrix4(projection_location, false, ref zoom);
                    GL.UniformMatrix4(model_view_location, false, ref Matrix4.Identity);

                    GL.EnableVertexAttribArray(0);
                    GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

                    GL.Viewport((int)(glControl1.Width / 2 - aspect_x * ratio) / 2 + glControl1.Width / 2, (int)(glControl1.Height - aspect_y * ratio) / 2,
                    (int)(aspect_x * ratio), (int)(aspect_y * ratio));

                    GL.DrawArrays(BeginMode.LineStrip, 0, polylines.Length);
                }

                /****/

                GL.UseProgram(0);


                glControl1.SwapBuffers();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {


            
            //DialogResult result = openFileDialog1.ShowDialog();
            string filename = @"C:\Users\Administrador\Desktop\dragon.grs";//openFileDialog1.FileName;
            Console.WriteLine(filename);
            
            string[] lines = System.IO.File.ReadAllLines(filename);

            // i stores the reading cursor
            int i = 0;
            while (lines[i] != "" && lines[i++][0] != '*') { }
            
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
            for(int j = 0; j < num_of_polylines; j++)
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
            projMatrix = Matrix4.CreateOrthographicOffCenter(bounds[0], bounds[2], bounds[3], bounds[1], -1f, 1f);
            zoomMatrix = Matrix4.CreateOrthographicOffCenter(bounds[0], bounds[2], bounds[3], bounds[1], -1f, 1f);


            // Define aspect ratio
            aspect_x = bounds[2] - bounds[0];
            aspect_y = bounds[1] - bounds[3];            

            glControl1.Invalidate();
        }

        private void glControl_Click(object sender, EventArgs e)

        {
            float fix_X,fix_Y;
            fix_X = 100;
            fix_Y = 50;
            MouseEventArgs mea = (MouseEventArgs)e;
            float  x = mea.X; 
            float y = (glControl1.Height -mea.Y);
            Vector3[] vertex = new Vector3[] {new Vector3(x,y, 0f),
                new Vector3(x + fix_X, y + fix_Y,0f), new Vector3(x +fix_X, y +fix_Y,0f), new Vector3(x + fix_X, y -fix_Y,0f)};
            Vector4 transf0 = new Vector4(unproj(vertex[0],Matrix4.Identity, projMatrix, new Rectangle(0,0,glControl1.Width/2,glControl1.Height)),1f);
            Vector4 transf1 = new Vector4(unproj(vertex[1], Matrix4.Identity, projMatrix, new Rectangle(0, 0, glControl1.Width / 2, glControl1.Height)), 1f);
            Vector4 transf2 = new Vector4(unproj(vertex[2], Matrix4.Identity, projMatrix, new Rectangle(0, 0, glControl1.Width / 2, glControl1.Height)), 1f);
            Vector4 transf3 = new Vector4(unproj(vertex[3], Matrix4.Identity, projMatrix, new Rectangle(0, 0, glControl1.Width / 2, glControl1.Height)), 1f);

            zoomMatrix = Matrix4.CreateOrthographicOffCenter(transf0.X,transf0.X + 0.25f,transf0.Y,transf0.Y + 0.35f,-1f,1f);

            glControl1.Invalidate();
        }



        Vector3 unproj(Vector3 win, Matrix4 mViewMat, Matrix4 projMat, Rectangle viewport)
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

    }
}


/* Square test 


                Vector4[] vert = new Vector4[]{
                    new Vector4(bounds[0] + 0.05f, bounds[3], 0f, 1f),
                    new Vector4(bounds[0] + 0.05f, bounds[1], 0f, 1f),
                    new Vector4(bounds[2], bounds[1], 0f, 1f),
                    new Vector4(bounds[2], bounds[3], 0f, 1f),
                };

                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(vert.Length * Vector4.SizeInBytes),
                                   vert, BufferUsageHint.StaticDraw);


                GL.UniformMatrix4(projection_location, false, ref translation);
                GL.UniformMatrix4(model_view_location, false, ref Matrix4.Identity);

                GL.EnableVertexAttribArray(0);
                GL.VertexAttribPointer(0, 4, VertexAttribPointerType.Float, false, 0, 0);

                GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
                GL.DrawArrays(BeginMode.LineLoop, 0, vert.Length);
*/