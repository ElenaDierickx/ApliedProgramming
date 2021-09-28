using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public interface ILogic
    {
        public int MandelbrotFractal(int X, int Y, int iterations, double zoom, int offsetX, int offsetY);
    }
}
