using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer
{
    public interface ILogic
    {
        public int[,] MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, double offsetX, double offsetY);
    }
}
