namespace Lab7
{
    partial class Lab7
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
            this.SuspendLayout();
            // 
            // openGLControl1
            // 
            this.openGLControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.openGLControl1.BackColor = System.Drawing.Color.Black;
            this.openGLControl1.Location = new System.Drawing.Point(1, 0);
            this.openGLControl1.Name = "openGLControl1";
            this.openGLControl1.Size = new System.Drawing.Size(627, 456);
            this.openGLControl1.TabIndex = 0;
            this.openGLControl1.VSync = false;
            this.openGLControl1.Load += new System.EventHandler(this.openGLControl1_Load);
            // 
            // Lab7
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(628, 456);
            this.Controls.Add(this.openGLControl1);
            this.Name = "Lab7";
            this.Text = "Lab7";
            this.ResumeLayout(false);

        }

        #endregion

        private Utilities.OpenGLControl openGLControl1;
    }
}

