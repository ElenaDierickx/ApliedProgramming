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
        public int[,] MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, double offsetX, double offsetY, CancellationToken tokenSource);
        public int[,] GreyScale(int maxRow, int maxColumn, int[,] mandelPoints, int iteration, CancellationToken tokenSource);
        public int[,] Banding(int maxRow, int maxColumn, int[,] mandelPoints, CancellationToken tokenSource);
        public int[,] UglyBanding(int maxRow, int maxColumn, int[,] mandelPoints, CancellationToken tokenSource);
        public int[,] Colors(int maxRow, int maxColumn, int[,] mandelPoints, int iteration, CancellationToken tokenSource);
        public DoublePoint Scaling(int X, int Y, int maxRow, int maxColumn, double zoom, double offsetX, double offsetY);
    }
}
