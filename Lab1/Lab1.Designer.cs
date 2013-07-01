namespace Lab1
{
	partial class Lab1
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
			this.openGLControl1.Location = new System.Drawing.Point(0, 0);
			this.openGLControl1.Name = "openGLControl1";
			this.openGLControl1.Size = new System.Drawing.Size(549, 414);
			this.openGLControl1.TabIndex = 0;
			this.openGLControl1.VSync = false;
			this.openGLControl1.Load += new System.EventHandler(this.glControl_Load);
			this.openGLControl1.Paint += new System.Windows.Forms.PaintEventHandler(this.glControl_Paint);
			// 
			// Lab1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(548, 413);
			this.Controls.Add(this.openGLControl1);
			this.Name = "Lab1";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.ResumeLayout(false);

		}

		#endregion

		private Utilities.OpenGLControl openGLControl1;
	}
}

