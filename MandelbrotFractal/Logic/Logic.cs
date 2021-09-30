using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class Logic : ILogic
    {
        //public int MandelbrotFractal(int X, int Y, int iterations) 
        //{
        //    double b = X / 150d - 2d;
        //    double a = Y / 200d - 2d;
        //    int iter = 1;
        //    double x = 0;
        //    double y = 0;
        //    double r = 0;
        //    while ((iter < iterations) && (r < 4))
        //    {
        //        double newX = Math.Pow(x, 2) - Math.Pow(y, 2) + a;
        //        double newY = 2 * x * y + b;
        //        x = newX;
        //        y = newY;
        //        r = x * x + y * y;
        //        iter++;
        //    }
        //    return iter;
        //}

        public List<DoublePoint> MandelbrotFractal(int iterations, int maxRow, int maxColumn)
        {
            var list = new List<DoublePoint>();
            for (int X = 0; X < maxRow; X++)
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    double b = X / 150d - 2d;
                    double a = Y / 200d - 2d;
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
                    list.Add(new DoublePoint(X, Y, iter));
                }
            }
            return list;
        }
    }

    
}
