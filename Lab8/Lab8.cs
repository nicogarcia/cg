using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Utilities;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Utilities.Shaders;
using Utilities.Shapes;

namespace Lab8
{
    public partial class Lab8 : Form
    {
        Foot foot;
        Cover cover;
        Cylinder cylinder;
        Cylinder cylinder1;
        Cylinder cylinder2;
        Cylinder cylinder3;
        Cone cone_x;
        Cone cone_y;
        Cone cone_z;
        Cylinder x_axis;
        Cylinder y_axis;
        Cylinder z_axis;
        Cylinder floor;

        public Lab8()
        {
            InitializeComponent();
        }

        private void openGLControl1_Load(object sender, EventArgs e)
        {
            ProgramObject program = new ProgramObject(
                new VertexShader(Shaders.VERTEX_SHADER_TEXTURE),
                    new FragmentShader(Shaders.FRAGMENT_SHADER_ILLUMINATION));
                    
            int num = 100;
            float radius = 1.0f;
            float height = radius * 4f;
            cylinder = new Cylinder(radius, height, num, new Vector4(0, 1f, 0, 1f), program);
            cylinder.transformation = Matrix4.CreateTranslation(-10f, 4f,0f);
            cylinder.colored = false;


            cylinder1 = new Cylinder(radius, height, num, new Vector4(0, 1f, 0, 1f), program);
            cylinder1.transformation = Matrix4.CreateTranslation(-10f, 4f, 0f);
            cylinder1.colored = false;


            cylinder2 = new Cylinder(radius, height, num, new Vector4(0, 1f, 0, 1f), program);
            cylinder2.transformation = Matrix4.CreateTranslation(-15f, 4f, 0f);
            cylinder2.colored = false;


            cylinder3 = new Cylinder(radius, height, num, new Vector4(0, 1f, 0, 1f), program);
            cylinder3.transformation = Matrix4.CreateTranslation(-20f, 4f, 0f);
            cylinder3.colored = false;
                        
            cover = new Cover(0.5f, 5, program);
            cover.colored = false;

            foot = new Foot(2f, 20, program);
            foot.colored = false;

            /*** 3D AXES ***/

            /* X */
            cone_x = new Cone(0.1f, 0.2f, 10, new Vector4(1, 0, 0, 1f), program);
            cone_x.transformation = Matrix4.CreateRotationY((float)Math.PI / 2);
            cone_x.transformation *= Matrix4.CreateTranslation(1f, 0f, 0f);
            x_axis = new Cylinder(0.05f, 1f,  10,new Vector4(1, 0, 0, 1f), program);
            x_axis.transformation = Matrix4.CreateRotationY((float)Math.PI / 2);

            /* Y */
            cone_y = new Cone(0.1f, 0.2f, 10, new Vector4(0, 1f, 0, 1f), program);
            cone_y.transformation = Matrix4.CreateRotationX((float)-Math.PI / 2);
            cone_y.transformation *= Matrix4.CreateTranslation(0f, 1f, 0f);
            y_axis = new Cylinder(0.05f, 1f, 10, new Vector4(0, 1f, 0, 1f), program);
            y_axis.transformation = Matrix4.CreateRotationX((float)-Math.PI / 2);

            /* Z */
            cone_z = new Cone(0.1f, 0.2f, 10, new Vector4(0, 0, 1f, 1f), program);
            cone_z.transformation = Matrix4.CreateTranslation(0f, 0f, 1f);

            z_axis = new Cylinder(0.05f, 1f, 10, new Vector4(0, 0, 1f, 1f), program);


            /**************/
            floor = new Cylinder(1000f, 0.1f, 4,new Vector4(0.5f, 0.5f, 0.5f, 1f), program);
            floor.colored = false;

            // Load texture
            Utilities.LoadImageTexture.LoadTexture(@"..\..\texture.jpg");

            // Add the objects to the selector
            objectSelector1.AddObject(cone_x);
            objectSelector1.AddObject(cone_y);
            objectSelector1.AddObject(cone_z);
            objectSelector1.AddObject(x_axis);
            objectSelector1.AddObject(y_axis);
            objectSelector1.AddObject(z_axis);
            objectSelector1.AddObject(foot);
            objectSelector1.AddObject(cover);
            objectSelector1.AddObject(cylinder);
            objectSelector1.AddObject(floor);
            objectSelector1.AddObject(cylinder1);
            objectSelector1.AddObject(cylinder2);
            objectSelector1.AddObject(cylinder3);

            objectSelector1.open_gl_control = openGLControl1;

            objectSelector1.SelectedIndices.Add(6);
            objectSelector1.SelectedIndices.Add(7);
            objectSelector1.SelectedIndices.Add(8);
            objectSelector1.SelectedIndices.Add(9);
            objectSelector1.SelectedIndices.Add(10);
            objectSelector1.SelectedIndices.Add(11);
            objectSelector1.SelectedIndices.Add(12);

            Viewport little_view = new Viewport(0, 0, 100, 100);
            little_view.AddObjects(objectSelector1.objects);
            little_view.setMatrices(Matrix4.CreatePerspectiveFieldOfView(1f, 1f,1f,100f), openGLControl1.camera.lookAt());
            openGLControl1.AddViewport(little_view);

            openGLControl1.load();
        }

        private void Lab8_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == (int) Keys.Escape)
                this.Dispose();
        }

    }
}
