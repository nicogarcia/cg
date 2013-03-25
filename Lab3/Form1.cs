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

namespace Lab3
{
    public partial class Form1 : Form
    {

        Vector4[][] vertices;

        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Yellow);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            DialogResult result = openFileDialog1.ShowDialog();
            string filename = openFileDialog1.FileName;
            Console.WriteLine(filename);
            
            string[] lines = System.IO.File.ReadAllLines(filename);

            // i stores the reading cursor
            int i = 0;
            while (lines[i] != "" && lines[i++][0] != '*') { }
            
            // At the line after '**************'

            // Replace double spaces by ony one
            lines[i] = lines[i].Replace("  ", " ");
            // Read the bounds of the image
            string[] bounds = lines[i++].Split(' ');

            float left, top, right, bottom;
            float.TryParse(bounds[0], out left);
            float.TryParse(bounds[1], out top);
            float.TryParse(bounds[2], out right);
            float.TryParse(bounds[3], out bottom);

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
        }

    }
}
