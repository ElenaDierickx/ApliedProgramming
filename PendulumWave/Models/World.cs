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
        private const double _worldSize = 0.6;

        public Point3D Origin => new();
        public (Point3D p1, Point3D p2) Bounds { get; private set; }
        public ImmutableList<Beam> Beams { get; private set; }
        public ImmutableList<Rope> Ropes { get; private set; }


        public World()
        {
            Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
            Ropes = ImmutableList<Rope>.Empty;
            Beams = ImmutableList<Beam>.Empty;
        }

        public void AddBeams(int amount)
        {
            Beams = Beams.Clear();
            Beam beam1 = new Beam
            {
                AnchorPoint = new Point3D() { X = 0, Y = 0, Z = -((double)amount + 5f) / 2f / 50f },
                Angle = -90,
                Length = ((double)amount + 5f) / 50f,
                RotationalDelta = 0
            };
            Beam beam2 = new Beam
            {
                AnchorPoint = new Point3D() { X = 0, Y = 0, Z = -(amount + 5) / 2f / 50f },
                Angle = 0,
                Length = 0.4f,
                RotationalDelta = -70
            };
            Beam beam3 = new Beam
            {
                AnchorPoint = new Point3D() { X = 0, Y = 0, Z = -(amount + 5) / 2f / 50f },
                Angle = 0,
                Length = 0.4f,
                RotationalDelta = -110
            };
            Beam beam4 = new Beam
            {
                AnchorPoint = new Point3D() { X = 0, Y = 0, Z = (amount + 5) / 2f / 50f },
                Angle = 0,
                Length = 0.4f,
                RotationalDelta = -70
            };
            Beam beam5 = new Beam
            {
                AnchorPoint = new Point3D() { X = 0, Y = 0, Z = (amount + 5) / 2f / 50f },
                Angle = 0,
                Length = 0.4f,
                RotationalDelta = -110
            };
            Beams = Beams.Add(beam1);
            Beams = Beams.Add(beam2);
            Beams = Beams.Add(beam3);
            Beams = Beams.Add(beam4);
            Beams = Beams.Add(beam5);
        }

        public void AddPendulumRope(int amount)
        {
            Ropes = Ropes.Clear();
            for(double i = 0; i < amount; i++)
            {
                double t = 60 / (50 + i) / (2f * Math.PI);
                Rope rope = new()
                {
                    AnchorPoint = new Point3D() { X = 0, Y = 0, Z = (i - amount / 2) / 50 },
                    Length = t * t * 9.81,
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
