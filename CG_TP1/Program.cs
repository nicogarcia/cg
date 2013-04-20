using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using CG_TP1.Shapes;
using System.IO;
using System.Reflection;
using Utilities;

namespace CG_TP1
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Application.Run(new MainWindow(getExercises()));
        }

        private static Exercise[] getExercises()
        {
            List<Exercise> exercises = new List<Exercise>();

            /*exercises.Add(new Exercise(
                "TP2-Rombo",
                delegate()
                {
                    Matrix4 transformacion = Matrix4.CreateTranslation(-0.5f, 0f, 0f);
                    Matrix4 transformacion1 = transformacion * Matrix4.CreateRotationZ((float) Math.PI/2) ;
                    Matrix4 transformacion2 = transformacion * Matrix4.CreateRotationZ((float) Math.PI) ;
                    Matrix4 transformacion3 = transformacion * Matrix4.CreateRotationZ((float) Math.PI*3/2) ;

                    return new Drawable2D[] { new Romboid(0.75f, 0.25f, transformacion),
                        new Romboid(0.75f, 0.25f, transformacion1),
                        new Romboid(0.75f, 0.25f, transformacion2),
                        new Romboid(0.75f, 0.25f, transformacion3)};
                }
            ));*/

            exercises.Add(new Exercise(
                "Ejercicio 1",
                delegate()
                {
                    return new Drawable2D[] { new Sierpinski(new OpenTK.Vector4[]{
                        new OpenTK.Vector4(-.75f, -.75f, 0f, 1f),
                        new OpenTK.Vector4(0, .75f, 0f, 1f),
                        new OpenTK.Vector4(.75f, -.75f, 0f, 1f)
                    })};
                }
            ));

            exercises.Add(new Exercise(
                "Ejercicio 2",
                delegate()
                {
                    string path = Path.Combine(Environment.CurrentDirectory, @"..\..\PolylineData.txt");

                    string[] lines = System.IO.File.ReadAllLines(path);

                    int count = lines.Length;

                    Vector4[] vertices = new Vector4[count];

                    for (int i = 0; i < count; i++)
                    {
                        string[] numbers = lines[i].Split(' ');
                        float x = float.Parse(numbers[0]);
                        float y = float.Parse(numbers[1]);

                        vertices[i] = new Vector4(x, y, 0f, 1f);
                    }

                    return new Drawable2D[] { new Polygon(vertices, false) };
                }
            ));

            exercises.Add(new Exercise(
                "Ejercicio 3.a.1",
                delegate()
                {
                    Vector4[] vertices = new Vector4[4];

                    vertices[0] = new Vector4(-0.5f, 0.5f, 0f, 1f);
                    vertices[1] = new Vector4(0.5f, 0.5f, 0f, 1f);
                    vertices[2] = new Vector4(0.25f, -0.5f, 0f, 1f);
                    vertices[3] = new Vector4(-0.25f, -0.5f, 0f, 1f);

                    return new Drawable2D[] { new Polygon(vertices, true) };
                }
            ));

            exercises.Add(new Exercise(
                "Ejercicio 3.b",
                delegate()
                {
                    return new Drawable2D[] { new RegularPolygon(8, 0.5f, true) };
                }
            ));

            exercises.Add(new Exercise(
                "Ejercicio 4.1",
                delegate()
                {
                    return new Drawable2D[] { new Cross(1f, 1f) };
                }
            ));

            exercises.Add(new Exercise(
                "Ejercicio 4.2",
                delegate()
                {
                    return new Drawable2D[] { new Star(.285f, .75f) };
                }
            ));

            return exercises.ToArray();
        }
    }
}
