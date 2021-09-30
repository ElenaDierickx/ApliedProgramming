using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Logic : ILogic
    {
        public int MandelbrotFractal(int X, int Y, int iterations, double zoom, int offsetX, int offsetY) 
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
            return iter;
        }
    }

    
}
