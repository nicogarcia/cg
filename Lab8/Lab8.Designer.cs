namespace Lab8
{
    partial class Lab8
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
			this.objectSelector1 = new Utilities.Controls.ObjectSelector();
			this.openGLControl1 = new Utilities.OpenGLControl();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// objectSelector1
			// 
			this.objectSelector1.CheckBoxes = true;
			this.objectSelector1.Dock = System.Windows.Forms.DockStyle.Left;
			this.objectSelector1.HoverSelection = true;
			this.objectSelector1.Location = new System.Drawing.Point(0, 0);
			this.objectSelector1.Name = "objectSelector1";
			this.objectSelector1.Size = new System.Drawing.Size(135, 419);
			this.objectSelector1.TabIndex = 3;
			this.objectSelector1.UseCompatibleStateImageBehavior = false;
			this.objectSelector1.View = System.Windows.Forms.View.List;
			this.objectSelector1.Visible = false;
			// 
			// openGLControl1
			// 
			this.openGLControl1.BackColor = System.Drawing.Color.Black;
			this.openGLControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.openGLControl1.Location = new System.Drawing.Point(0, 0);
			this.openGLControl1.Name = "openGLControl1";
			this.openGLControl1.Size = new System.Drawing.Size(519, 419);
			this.openGLControl1.TabIndex = 0;
			this.openGLControl1.VSync = false;
			this.openGLControl1.Load += new System.EventHandler(this.openGLControl1_Load);
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.BackColor = System.Drawing.Color.Transparent;
			this.label1.Font = new System.Drawing.Font("Century Gothic", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(345, 373);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(175, 48);
			this.label1.TabIndex = 4;
			this.label1.Text = "label1";
			// 
			// Lab8
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 419);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.objectSelector1);
			this.Controls.Add(this.openGLControl1);
			this.KeyPreview = true;
			this.Name = "Lab8";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Lab8_KeyUp);
			this.ResumeLayout(false);

        }

        #endregion

        private Utilities.OpenGLControl openGLControl1;
        private Utilities.Controls.ObjectSelector objectSelector1;
        private System.Windows.Forms.Label label1;
    }
}

