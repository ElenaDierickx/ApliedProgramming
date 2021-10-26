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
        private const int _worldSize = 10;

        public Point3D Origin => new();
        public (Point3D p1, Point3D p2) Bounds { get; private set; }
        public Beam Beam { get; private set; }
        public ImmutableList<Sphere> Spheres { get; private set; }
        public ImmutableList<Rope> Ropes { get; private set; }


        public World()
        {
            Bounds = (new Point3D(-_worldSize / 2, -_worldSize / 2, -_worldSize / 2),
                      new Point3D(_worldSize / 2, _worldSize / 2, _worldSize / 2));
            Spheres = ImmutableList<Sphere>.Empty;
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

        public void AddSphere()
        {
            Sphere sphere = new()
            {
                Position = new Point3D() { X = 0, Y = 20, Z = 0 },
                Speed = new Vector3D { X = 0, Y = 0, Z = 0 },
                Acceleration = new Vector3D { X = 0, Y = -9.81, Z = 0 }
            };
            Spheres = Spheres.Add(sphere);
        }

        //public void AddSphere()
        //{
        //    Sphere sphere = new()
        //    {
        //        Position = new Point3D() { X = 0, Y = 0, Z = 0 },
        //        Speed = new Vector3D { X = 0, Y = 0, Z = 0 },
        //        Acceleration = new Vector3D { X = 0, Y = -9.81, Z = 0 }
        //    };
        //    Rope rope = new()
        //    {
        //        AnchorPoint = new Point3D() { X = 0, Y = 0, Z = 0 },
        //        Length = 5,
        //        Angle = 90

        //    };
        //    Spheres = Spheres.Add(sphere);
        //    Ropes = Ropes.Add(rope);
        //}

        public void UpdateSpheres(double DeltaT)
        {
            foreach(Sphere sphere in Spheres)
            {
                sphere.UpdateSphere(DeltaT);
            }
        }
    }
}
