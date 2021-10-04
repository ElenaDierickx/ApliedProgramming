﻿using System;
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
                    double b = (X / 600d * 4d - 2d) * zoom + (offsetY / 600d) * zoom;
                    double a = (Y / 800d * 4d - 2d) * zoom + (offsetX / 800d) * zoom;
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
    }

    
}
