﻿using System;
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
        public Beam Beam { get; }
        ImmutableList<Point3D> SpherePositions { get; }
        public void AddSphere();
    }
}
