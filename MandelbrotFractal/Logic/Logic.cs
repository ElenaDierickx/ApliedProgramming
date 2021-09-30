using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Logic : ILogic
    {
        public List<MandelPoint> MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, int offsetX, int offsetY)
        {
            var list = new List<MandelPoint>();
            for (int X = 0; X < maxRow; X++)
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    double b = ((X + offsetY) / (150d * zoom) - 2d);
                    double a = (Y + offsetX) / (200d * zoom) - 2d;
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
                    list.Add(new MandelPoint(X, Y, iter));
                }
            }
            return list;
        }
    }

    
}
