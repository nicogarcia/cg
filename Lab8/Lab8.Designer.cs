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
            this.SuspendLayout();
            // 
            // objectSelector1
            // 
            this.objectSelector1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.objectSelector1.CheckBoxes = true;
            this.objectSelector1.HoverSelection = true;
            this.objectSelector1.Location = new System.Drawing.Point(0, -1);
            this.objectSelector1.Name = "objectSelector1";
            this.objectSelector1.Size = new System.Drawing.Size(135, 422);
            this.objectSelector1.TabIndex = 3;
            this.objectSelector1.UseCompatibleStateImageBehavior = false;
            this.objectSelector1.View = System.Windows.Forms.View.List;
            // 
            // openGLControl1
            // 
            this.openGLControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControl1.BackColor = System.Drawing.Color.Black;
            this.openGLControl1.Location = new System.Drawing.Point(141, -1);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.Size = new System.Drawing.Size(379, 422);
            this.openGLControl1.TabIndex = 0;
            this.openGLControl1.VSync = false;
            this.openGLControl1.Load += new System.EventHandler(this.openGLControl1_Load);
            // 
            // Lab8
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 419);
            this.Controls.Add(this.objectSelector1);
            this.Controls.Add(this.openGLControl1);
            this.Name = "Lab8";
            this.Text = "Form1";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.ResumeLayout(false);

        }

        #endregion

        private Utilities.OpenGLControl openGLControl1;
        private Utilities.Controls.ObjectSelector objectSelector1;
    }
}

