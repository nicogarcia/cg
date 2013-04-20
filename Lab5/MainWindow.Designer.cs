namespace Lab5
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.openGLControl1 = new Utilities.OpenGLControl();
            this.glControl1 = new OpenTK.GLControl();
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.openGLControl1.BackColor = System.Drawing.Color.Black;
            this.openGLControl1.Location = new System.Drawing.Point(440, 12);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.Size = new System.Drawing.Size(426, 514);
            this.openGLControl1.TabIndex = 3;
            this.openGLControl1.VSync = false;
            this.openGLControl1.Load += new System.EventHandler(this.OpenGLControl_Load);
            this.openGLControl1.Resize += new System.EventHandler(this.resize);
            // 
            // glControl1
            // 
            this.glControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.glControl1.BackColor = System.Drawing.Color.Black;
            this.glControl1.Location = new System.Drawing.Point(-3, 0);
            this.glControl1.Name = "glControl1";
            this.glControl1.Size = new System.Drawing.Size(437, 538);
            this.glControl1.TabIndex = 0;
            this.glControl1.VSync = false;
            this.glControl1.Load += new System.EventHandler(this.glControl_Load);
            this.glControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
            this.glControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.glControl_KeyPress);
            this.glControl1.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.glControl_scroll);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 538);
            this.Controls.Add(this.openGLControl1);
            this.Controls.Add(this.glControl1);
            this.Name = "MainWindow";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Utilities.OpenGLControl openGLControl1;
        private OpenTK.GLControl glControl1;
    }
}

