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
        Matrix4 projMatrix = Matrix4.Scale(0.7f) * Matrix4.CreateRotationY((float)Math.PI / 8f) * Matrix4.CreateRotationX((float)Math.PI / 8f);
        Matrix4 zoomMatrix = Matrix4.Identity;

        PolyNet polynet = new PolyNet();
        Vector4[] toDraw;

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
            });

            toDraw = new Vector4[polynet.faces.Count * 4];

            int cursor = 0;
            foreach (Face face in polynet.faces)
            {
                // Add face's vertices to "toDraw" array
                Vector4[] array = face.vertices();
                for (int i = 0; i < array.Length; i++)
                {
                    toDraw[cursor++] = array[i];
                }
            }

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

                GL.UniformMatrix4(projection_location, false, ref projMatrix);
                GL.UniformMatrix4(model_view_location, false, ref zoomMatrix);

                for (int i = 0; i < 6; i++)
                {
                    GL.DrawArrays(BeginMode.LineLoop, i * 4, 4);
                }

                GL.UseProgram(0);

                glControl1.SwapBuffers();
            }
        }
    }
}
