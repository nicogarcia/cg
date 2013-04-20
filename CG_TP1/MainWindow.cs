using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities;

namespace CG_TP1
{
    public partial class MainWindow : Form
    {
        private Drawable2D[] figures;
        private Exercise[] functions;

        public MainWindow(Exercise[] functions)
        {
            this.functions = functions;
            InitializeComponent();
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            GL.ClearColor(Color.Yellow);
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            if (figures != null)
                for (int i = 0; i < figures.Length; i++ )
                {
                    figures[i].paint();
                }

            glControl1.SwapBuffers();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(functions);
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            figures = ((Exercise)comboBox1.SelectedItem).run();
            glControl1.Refresh();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            int aspect_y = 1, aspect_x = 1;

            // Calculate resize ratios for resizing
            int ratioW = glControl1.Width / aspect_x;
            int ratioH = glControl1.Height / aspect_y;

            // smaller ratio will ensure that the image fits in the view
            int ratio = ratioW < ratioH ? ratioW : ratioH;

            GL.Viewport((glControl1.Width - aspect_x * ratio) / 2, (glControl1.Height - aspect_y * ratio) / 2,
                aspect_x * ratio, aspect_y * ratio);
            
            glControl1.Invalidate();
        }

    }
}
