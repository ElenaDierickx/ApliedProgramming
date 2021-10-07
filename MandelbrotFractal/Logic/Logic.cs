using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LogicLayer
{
        
    public class Logic : ILogic
    {
        public int[,] MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, double offsetX, double offsetY)
        {
            int[,] mandel = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X) =>
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    double b = (X / (double)maxRow * 4d - 2d) / zoom + offsetY;
                    double a = (Y / (double)maxColumn * 4d - 2d) / zoom + offsetX;
                    int iter = 1;
                    double x = 0;
                    double y = 0;
                    double r = 0;
                    while ((iter < iterations) && (r < 4))
                    {
                        double newX = x * x - y * y + a;
                        double newY = 2 * x * y + b;
                        x = newX;
                        y = newY;
                        r = x * x + y * y;
                        iter++;
                    }
                    lock (mandel)
                        mandel[X, Y] = iter;
                }
            });
            return mandel;
        }

        public DoublePoint Scaling(int X, int Y, int maxRow, int maxColumn, double zoom, double offsetX, double offsetY)
        {
            double a = Math.Round((X / (double)maxColumn * 4d - 2d) / zoom + offsetY, 2);
            double b = Math.Round((Y / (double)maxRow * 4d - 2d) / zoom + offsetX, 2);
            return new DoublePoint(a, b);
        }

        public int[,] GreyScale(int maxRow, int maxColumn, int[,] mandelPoints, int iteration)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X) =>
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    byte colorValue = (byte)((double)mandelPoints[X, Y] / (double)iteration * 255d);
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { colorValue, colorValue, colorValue, 255 });
                }
            });
            return colorInts;
        }

        public int[,] Banding(int maxRow, int maxColumn, int[,] mandelPoints)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X) =>
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    byte colorValue = 0;
                    if (mandelPoints[X, Y] % 2 != 0)
                    {
                        colorValue = 255;
                    }
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { colorValue, colorValue, colorValue, 255 });
                }
            });
            return colorInts;
        }

        private readonly List<Color> colors = new List<Color> { Color.White, Color.Purple, Color.MediumPurple, Color.Black };
        public int[,] UglyBanding(int maxRow, int maxColumn, int[,] mandelPoints)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X) =>
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    Color color = colors[0];
                    if (mandelPoints[X, Y] % 4 == 0)
                    {
                        color = colors[2];
                    }
                    else if (mandelPoints[X, Y] % 3 == 0)
                    {
                        color = colors[1];
                    }
                    else if (mandelPoints[X, Y] % 2 == 0)
                    {
                        color = colors[0];
                    }
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { color.B, color.G, color.R, color.A });
                }
            });
            return colorInts;
        }

        public int[,] Colors(int maxRow, int maxColumn, int[,] mandelPoints, int iteration)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X) =>
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    double colorValue = ((double)mandelPoints[X, Y] / (double)iteration * 255d);
                    if (mandelPoints[X, Y] == iteration)
                        colorValue = 0;
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { (byte)(colorValue % 3 * 85), (byte)(colorValue % 4 * 63), (byte)(colorValue % 5 * 51), 255 });
                }
            });
            return colorInts;
        }

    }

    
}
