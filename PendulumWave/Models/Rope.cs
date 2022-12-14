using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Models
{
    public class Rope
    {
        public Point3D AnchorPoint { get; set; }
        public double Length { get; set; }
        public double Angle { get; set; }
        public double AngleSpeed { get; set; }
        public double AngleAcceleration { get; set; }

        public void UpdateRope(double deltaT)
        {
            Angle += AngleSpeed * deltaT + (deltaT * deltaT / 2f) * AngleAcceleration;
            AngleSpeed += AngleAcceleration * deltaT;
            AngleAcceleration = -(9.81 / Length) * Math.Sin(Angle * (Math.PI / 180));
        }
    }

    
}
