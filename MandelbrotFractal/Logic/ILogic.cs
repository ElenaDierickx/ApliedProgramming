using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public interface ILogic
    {
        public List<MandelPoint> MandelbrotFractal(int maxRow, int maxColumn, int iterations, double zoom, int offsetX, int offsetY);
    }
}
