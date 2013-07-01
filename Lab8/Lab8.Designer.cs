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
			this.openGLControl1 = new Utilities.OpenGLControl();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btn_table = new System.Windows.Forms.Button();
			this.btn_cylinder = new System.Windows.Forms.Button();
			this.chk_draw_normals = new System.Windows.Forms.CheckBox();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
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
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.chk_draw_normals);
			this.panel1.Controls.Add(this.btn_table);
			this.panel1.Controls.Add(this.btn_cylinder);
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(519, 44);
			this.panel1.TabIndex = 1;
			// 
			// btn_table
			// 
			this.btn_table.Location = new System.Drawing.Point(93, 12);
			this.btn_table.Name = "btn_table";
			this.btn_table.Size = new System.Drawing.Size(75, 23);
			this.btn_table.TabIndex = 1;
			this.btn_table.Text = "Mesa";
			this.btn_table.UseVisualStyleBackColor = true;
			this.btn_table.Click += new System.EventHandler(this.btn_table_Click);
			// 
			// btn_cylinder
			// 
			this.btn_cylinder.Location = new System.Drawing.Point(12, 12);
			this.btn_cylinder.Name = "btn_cylinder";
			this.btn_cylinder.Size = new System.Drawing.Size(75, 23);
			this.btn_cylinder.TabIndex = 0;
			this.btn_cylinder.Text = "Cilindro";
			this.btn_cylinder.UseVisualStyleBackColor = true;
			this.btn_cylinder.Click += new System.EventHandler(this.btn_cylinder_Click);
			// 
			// chk_draw_normals
			// 
			this.chk_draw_normals.AutoSize = true;
			this.chk_draw_normals.Location = new System.Drawing.Point(174, 16);
			this.chk_draw_normals.Name = "chk_draw_normals";
			this.chk_draw_normals.Size = new System.Drawing.Size(104, 17);
			this.chk_draw_normals.TabIndex = 2;
			this.chk_draw_normals.Text = "Dibujar normales";
			this.chk_draw_normals.UseVisualStyleBackColor = true;
			this.chk_draw_normals.CheckedChanged += new System.EventHandler(this.chk_draw_normals_CheckedChanged);
			// 
			// Lab8
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(519, 419);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.openGLControl1);
			this.KeyPreview = true;
			this.Name = "Lab8";
			this.Text = "Form1";
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Lab8_KeyUp);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

		private Utilities.OpenGLControl openGLControl1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btn_cylinder;
		private System.Windows.Forms.Button btn_table;
		private System.Windows.Forms.CheckBox chk_draw_normals;
    }
}

