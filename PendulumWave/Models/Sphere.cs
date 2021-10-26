using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Models
{
    public class Sphere
    {
        public Point3D Position { get; set; }
        public Vector3D Speed { get; set; }
        public Vector3D Acceleration { get; set; }

        public void UpdateSphere(double DeltaT)
        {
            Position += Speed * DeltaT + (DeltaT * DeltaT / 2f) * Acceleration;
            Speed += Acceleration * DeltaT;
        }

    }

    
}
