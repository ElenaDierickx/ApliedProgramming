using System;
using System.Collections.Generic;
using System.Windows;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;

namespace LogicLayer
{
    public interface ILogic
    {
        public int[,] MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, double offsetX, double offsetY);
        public int[,] GreyScale(int maxRow, int maxColumn, int[,] mandelPoints, int iteration);
        public int[,] Banding(int maxRow, int maxColumn, int[,] mandelPoints);
        public int[,] UglyBanding(int maxRow, int maxColumn, int[,] mandelPoints);
        public int[,] Colors(int maxRow, int maxColumn, int[,] mandelPoints, int iteration);
        public DoublePoint Scaling(int X, int Y, int maxRow, int maxColumn, double zoom, double offsetX, double offsetY);
    }
}
