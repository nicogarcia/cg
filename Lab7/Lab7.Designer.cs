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
			this.panel1 = new System.Windows.Forms.Panel();
			this.btn_nayar = new System.Windows.Forms.Button();
			this.btn_cook = new System.Windows.Forms.Button();
			this.btn_phong = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// openGLControl1
			// 
			this.openGLControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.openGLControl1.BackColor = System.Drawing.Color.Black;
			this.openGLControl1.Location = new System.Drawing.Point(-3, -3);
			this.openGLControl1.Name = "openGLControl1";
			this.openGLControl1.Size = new System.Drawing.Size(756, 662);
			this.openGLControl1.TabIndex = 0;
			this.openGLControl1.VSync = false;
			this.openGLControl1.Load += new System.EventHandler(this.openGLControl1_Load);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.btn_nayar);
			this.panel1.Controls.Add(this.btn_cook);
			this.panel1.Controls.Add(this.btn_phong);
			this.panel1.Location = new System.Drawing.Point(-3, -3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(756, 44);
			this.panel1.TabIndex = 3;
			// 
			// btn_nayar
			// 
			this.btn_nayar.Location = new System.Drawing.Point(204, 12);
			this.btn_nayar.Name = "btn_nayar";
			this.btn_nayar.Size = new System.Drawing.Size(105, 23);
			this.btn_nayar.TabIndex = 3;
			this.btn_nayar.Text = "Oren Nayar";
			this.btn_nayar.UseVisualStyleBackColor = true;
			this.btn_nayar.Click += new System.EventHandler(this.btn_nayar_Click);
			// 
			// btn_cook
			// 
			this.btn_cook.Location = new System.Drawing.Point(93, 12);
			this.btn_cook.Name = "btn_cook";
			this.btn_cook.Size = new System.Drawing.Size(105, 23);
			this.btn_cook.TabIndex = 1;
			this.btn_cook.Text = "Cook Torrance";
			this.btn_cook.UseVisualStyleBackColor = true;
			this.btn_cook.Click += new System.EventHandler(this.btn_cook_Click);
			// 
			// btn_phong
			// 
			this.btn_phong.Location = new System.Drawing.Point(12, 12);
			this.btn_phong.Name = "btn_phong";
			this.btn_phong.Size = new System.Drawing.Size(75, 23);
			this.btn_phong.TabIndex = 0;
			this.btn_phong.Text = "Phong";
			this.btn_phong.UseVisualStyleBackColor = true;
			this.btn_phong.Click += new System.EventHandler(this.btn_phong_Click);
			// 
			// Lab7
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(748, 655);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.openGLControl1);
			this.Name = "Lab7";
			this.Text = "Form1";
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private Utilities.OpenGLControl openGLControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btn_nayar;
		private System.Windows.Forms.Button btn_cook;
		private System.Windows.Forms.Button btn_phong;
	}
}

