using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shape_Renderer
{
    public partial class Form1 : Form
    {
        static double theta = Math.PI / 16;

        static Color[] Colors;

        static float[] Light_Coordinates = { 500, 0, 0 };

        double[,] Rotation_x =
        {
                {1, 0, 0},

                {0, Math.Cos(theta), -Math.Sin(theta)},

                {0, Math.Sin(theta), Math.Cos(theta)},
        };

        double[,] Rotation_y =
{
                {Math.Cos(theta), 0, Math.Sin(theta)},

                {0, 1, 0},

                {-Math.Sin(theta), 0, Math.Cos(theta)},
        };

        double[,] Rotation_z =
        {
                {Math.Cos(theta), -Math.Sin(theta), 0},

                {Math.Sin(theta), Math.Cos(theta), 0},

                {0, 0, 1},
        };


        double[,] Cube_1;

        double[,] Torus_1;

        double[,] Sphere_1;

        double[,] Mobius_1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Torus_1 = Torus(400, 65);
        }

        static double[,] Cube(int Width)
        {
            double[,] Cube = new double[(int) (Math.Pow(Width, 3) - Math.Pow(Width - 2, 3)), 3];

            Colors = new Color[Cube.GetLength(0)];

            int w = 0;

            for (int x = 0; x <= Width - 1; x++)
            {
                for (int y = 0; y <= Width - 1; y++)
                {
                    for (int z = 0; z <= Width - 1; z++)
                    {
                        if(x == 0 || x == Width - 1 || y == 0 || y == Width - 1 || z == 0 || z == Width - 1)
                        {
                            for (int l = 0; l <= Cube.GetLength(1) - 1; l++)
                            {
                                switch (l)
                                {
                                    case 0:
                                        Cube[w, l] = x - (Width / 2);
                                        break;
                                    case 1:
                                        Cube[w, l] = y - (Width / 2);
                                        break;
                                    case 2:
                                        Cube[w, l] = z - (Width / 2);
                                        break;
                                }
                            }

                            if (x == 0 && y != 0 && y != Width - 1 && z != 0 && z != Width - 1)
                            {
                                Colors[w] = Color.DarkRed;
                            }
                            else if(x == Width - 1 && y != 0 && y != Width - 1 && z != 0 && z != Width - 1)
                            {
                                Colors[w] = Color.LightGreen;
                            }
                            else if (y == 0 && x != 0 && x != Width - 1 && z != 0 && z != Width - 1)
                            {
                                Colors[w] = Color.Aqua;
                            }
                            else if (y == Width - 1 && x != 0 && x != Width - 1 && z != 0 && z != Width - 1)
                            {
                                Colors[w] = Color.White;
                            }
                            else if (z == 0 && x != 0 && x != Width - 1 && y != 0 && y != Width - 1)
                            {
                                Colors[w] = Color.Violet;
                            }
                            else if (z == Width - 1 && x != 0 && x != Width - 1 && y != 0 && y != Width - 1)
                            {
                                Colors[w] = Color.Gray;
                            }
                            else
                            {
                                Colors[w] = Color.Black;
                            }

                            w++;
                        }
                    }
                }
            }

            return Cube;
        }

        static double[,] Torus(int Radius, int Thickness)
        {
            double[,] Torus = new double[(int) Math.Pow(628,2), 3];

            Colors = new Color[Torus.GetLength(0)];

            int w = 0;

            for (double j = 0; j < 628; j++)
            {
                for (double i = 0; i < 628; i++)
                {

                    Colors[w] = Color.SkyBlue;


                    for (int l = 0; l < Torus.GetLength(1); l++)
                    {
                        switch (l)
                        {
                            case 0:
                                Torus[w, l] = Thickness * Math.Cos(i/100) + Radius * Math.Cos(j/100);
                                break;
                            case 1:
                                Torus[w, l] = Thickness * Math.Sin(i/100);
                                break;
                            case 2:
                                Torus[w, l] = Radius * Math.Sin(j/100);
                                break;
                        }
                    }

                    w++;
                }
            }

            return Torus;
        }

        static double[,] Mobius(int Radius, int Width)
        {
            double[,] Mobius = new double[628 * (2 * Width + 1), 3];

            Colors = new Color[Mobius.GetLength(0)];

            int w = 0;

            for (double i = 0; i < 628; i++)
            {
                for (double j = -Width; j <= Width; j++)
                {

                    Colors[w] = Color.Blue;


                    for (int l = 0; l < Mobius.GetLength(1); l++)
                    {
                        switch (l)
                        {
                            case 0:
                                Mobius[w, l] = (Radius + j * Math.Cos(i/2)) * Math.Cos(i);
                                break;
                            case 1:
                                Mobius[w, l] = (Radius + j * Math.Cos(i / 2)) * Math.Sin(i);
                                break;
                            case 2:
                                Mobius[w, l] = j * Math.Sin(i/2);
                                break;
                        }
                    }

                    w++;
                }
            }

            return Mobius;
        }

        static double[,] Sphere(int Radius)
        {
            double[,] Sphere = new double[314 * 628, 3];

            Colors = new Color[Sphere.GetLength(0)];

            int w = 0;

            for (double j = 0; j < 314; j++)
            {
                for (double i = 0; i < 628; i++)
                {

                    Colors[w] = Color.SkyBlue;


                    for (int l = 0; l < Sphere.GetLength(1); l++)
                    {
                        switch (l)
                        {
                            case 0:
                                Sphere[w, l] = Radius * Math.Cos(i / 100) * Math.Cos(j / 100);
                                break;
                            case 1:
                                Sphere[w, l] = Radius * Math.Sin(i / 100);
                                break;
                            case 2:
                                Sphere[w, l] = -Radius * Math.Sin(j / 100) * Math.Cos(i / 100);
                                break;
                        }
                    }

                    w++;
                }
            }

            return Sphere;
        }

        static Bitmap Frame(double[,] Shape, int x_Coord, int y_Coord)
        {
            Bitmap Bitt = new Bitmap(1000, 1000);

            for(int y = 0; y < Bitt.Height; y++)
            {
                for(int x = 0; x < Bitt.Width; x++)
                {
                    Bitt.SetPixel(x, y, Color.White);
                }
            }

            for(int i = 0; i < Shape.GetLength(0); i++) 
            {
                double Distance_from_Light = Math.Sqrt((Math.Pow(Shape[i, 0] + x_Coord - Light_Coordinates[0], 2)) + (Math.Pow(Shape[i, 1] + y_Coord - Light_Coordinates[1], 2)) + (Math.Pow(Shape[i, 2] - Light_Coordinates[2], 2)));

                 Bitt.SetPixel((int)Shape[i, 0] + x_Coord, (int)Shape[i, 1] + y_Coord, Color.FromArgb(255, (int) ((-(double)Colors[i].R/1000)* Distance_from_Light + Colors[i].R), (int)((-(double)Colors[i].G / 1000) * Distance_from_Light + Colors[i].G), (int)((-(double)Colors[i].B / 1000) * Distance_from_Light + Colors[i].B)));
            }

            return Bitt;
        }

        static double[,] Multiplication(double[,] Matrix1, double[,] Matrix2)
        {
            if (Matrix1.GetLength(1) != Matrix2.GetLength(0))
            {
                Console.WriteLine("Invalid Dimensions.");

                return null;
            }
            else
            {
                double[,] New_Matrix = new double[Matrix1.GetLength(0), Matrix2.GetLength(1)];

                for (int i = 0; i <= New_Matrix.GetLength(0) - 1; i++)
                {
                    for (int j = 0; j <= New_Matrix.GetLength(1) - 1; j++)
                    {
                        double Dot_Product = 0;

                        for (int k = 0; k <= Matrix1.GetLength(1) - 1; k++)
                        {
                            Dot_Product += Matrix1[i, k] * Matrix2[k, j];
                        }

                        New_Matrix[i, j] = Dot_Product;

                    }
                }

                return New_Matrix;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            pictureBox1.Image = Frame(Torus_1, 500, 500);

            Torus_1 = Multiplication(Torus_1, Multiplication(Rotation_z, Multiplication(Rotation_y, Rotation_x)));
        }
    }
}
