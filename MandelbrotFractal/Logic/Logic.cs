using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public class Logic : ILogic
    {
        public int[,] MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, double offsetX, double offsetY)
        {
            int[,] mandel = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X, state) =>
            {
                Parallel.For(0, maxColumn, (Y, state) =>
                {
                    double b = (X / (double)maxColumn * 4d - 2d) * zoom + (offsetY / (double)maxColumn) * zoom;
                    double a = (Y / (double)maxRow * 4d - 2d) * zoom + (offsetX / (double)maxRow) * zoom;
                    int iter = 1;
                    double x = 0;
                    double y = 0;
                    double r = 0;
                    while ((iter < iterations) && (r < 4))
                    {
                        double newX = Math.Pow(x, 2) - Math.Pow(y, 2) + a;
                        double newY = 2 * x * y + b;
                        x = newX;
                        y = newY;
                        r = x * x + y * y;
                        iter++;
                    }
                    lock (mandel)
                        mandel[X, Y] = iter;
                });
            });
            return mandel;
        }

        public int[,] GreyScale(int maxRow, int maxColumn, int[,] mandelPoints, int iteration)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X, state) =>
            {
                Parallel.For(0, maxColumn, (Y, state) =>
                {
                    byte colorValue = (byte)(mandelPoints[X, Y] / iteration * 255d);
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { colorValue, colorValue, colorValue, 255 });
                });
            });
            return colorInts;
        }

        public int[,] BlueScale(int maxRow, int maxColumn, int[,] mandelPoints, int iteration)
        {
            int[,] colorInts = new int[maxRow, maxColumn];
            Parallel.For(0, maxRow, (X, state) =>
            {
                Parallel.For(0, maxColumn, (Y, state) =>
                {
                    byte colorValue = (byte)(mandelPoints[X, Y] / iteration * 255d);
                    colorInts[X, Y] = BitConverter.ToInt32(new byte[] { colorValue, 0, 0, 255 });
                });
            });
            return colorInts;
        }
    }

    
}
