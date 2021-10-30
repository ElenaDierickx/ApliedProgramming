using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Models
{
    public interface IWorld
    {
        Point3D Origin { get; }
        (Point3D p1, Point3D p2) Bounds { get; }
        ImmutableList<Beam> Beams { get; }
        ImmutableList<Rope> Ropes { get; }

        public void AddBeams(int amount);
        public void AddPendulumRope(int amount);

        public void UpdatePendulumRopes(double DeltaT);
    }
}
