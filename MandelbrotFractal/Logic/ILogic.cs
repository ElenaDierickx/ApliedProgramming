using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public interface ILogic
    {
        public List<DoublePoint> MandelbrotFractal(int iterations, int maxRow, int maxColumn);
    }
}
