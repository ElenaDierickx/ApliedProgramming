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
        private const int _worldSize = 15;

        public Point3D Origin => new();
        public (Point3D p1, Point3D p2) Bounds { get; private set; }
        public Beam Beam { get; private set; }
        public ImmutableList<Rope> Ropes { get; private set; }


        public World()
        {
            Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
            Ropes = ImmutableList<Rope>.Empty;
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

        public void AddPendulumRope(int amount)
        {
            for(int i = 0; i < amount; i++)
            {

                Rope rope = new()
                {
                    AnchorPoint = new Point3D() { X = 0, Y = 5, Z = i * 2 },
                    Length = 2 * Math.PI * Math.Sqrt(1.2 * (i + 1) / 9.81),
                    Angle = 40
                };
                Ropes = Ropes.Add(rope);
            }

        }

        public void UpdatePendulumRopes(double DeltaT)
        {
            foreach (Rope rope in Ropes)
            {
                rope.UpdateRope(DeltaT);
            }
        }
    }
}
