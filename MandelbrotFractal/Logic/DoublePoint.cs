using System;
using System.Collections.Generic;
using System.Text;

namespace LogicLayer
{
    public class DoublePoint
    {
        public int X { get; set; }
        public int Y { get; set; }

        public double iter { get; set; }

        public DoublePoint(int X, int Y, double iter)
        {
            this.X = X;
            this.Y = Y;
            this.iter = iter;
        }


    }
}
