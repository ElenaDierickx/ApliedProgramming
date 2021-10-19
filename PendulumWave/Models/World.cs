using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace Models
{
    public class World : IWorld
    {
        private const int _worldSize = 1000;

        public Point3D Origin => new();
        public (Point3D p1, Point3D p2) Bounds { get; private set; }
        public Beam Beam { get; private set; }
        public ImmutableList<Point3D> SpherePositions { get; private set; }


        public World()
        {
            Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
            SpherePositions = ImmutableList<Point3D>.Empty;
            InitBeam();
        }

        private void InitBeam()
        {
            Beam = new Beam
            {
                AnchorPoint = new Point3D { X = -1000, Y = 400, Z = 0 },
                Angle = 0,
                Length = 2000,
                RotationalDelta = 0
            };
        }

        public void AddSphere()
        {
            var position = new Point3D { X = 0, Y = -200, Z = 0 };
            SpherePositions = SpherePositions.Add(position);
        }
    }
}
