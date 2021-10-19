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
        public ImmutableList<Point3D> SpherePositions { get; private set; }
        private readonly Random _rnd = new();


        public World()
        {
            Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
            SpherePositions = ImmutableList<Point3D>.Empty;
        }

        private Point3D GetRandomPosition()
        {
            return new Point3D
            {
                X = Bounds.p1.X + ((Bounds.p2.X - Bounds.p1.X) * _rnd.NextDouble()),
                Y = Bounds.p1.Y + ((Bounds.p2.Y - Bounds.p1.Y) * _rnd.NextDouble()),
                Z = Bounds.p1.Z + ((Bounds.p2.Z - Bounds.p1.Z) * _rnd.NextDouble())
            };
        }

        public void AddSphere()
        {
            var position = GetRandomPosition();
            SpherePositions = SpherePositions.Add(position);
        }
    }
}
