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

namespace CG_TP1
{
    public partial class MainWindow : Form
    {
        private Drawable drawable;
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

            if(drawable != null)
                drawable.paint();

            glControl1.SwapBuffers();
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            comboBox1.Items.AddRange(functions);
            comboBox1.SelectedItem = comboBox1.Items[0];
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            drawable = ((Exercise)comboBox1.SelectedItem).run();
            glControl1.Refresh();
        }

        private void glControl_Resize(object sender, EventArgs e)
        {
            GLManager.refreshViewport(this.Width, this.Height);
            glControl1.Invalidate();
        }

    }
}
