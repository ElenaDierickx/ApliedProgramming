using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using Wpf3DUtils;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private readonly IWorld _world;
        private readonly ICameraController _cameraController;

        private readonly Model3DGroup _model3dGroup = new();
        public ProjectionCamera Camera => _cameraController.Camera;
        public Model3D Visual3dContent => _model3dGroup;
        private readonly Model3DGroup _sphereGroup = new();

        private readonly Model3DGroup _ropeGroup = new();
        private GeometryModel3D _beam;

        

        public IRelayCommand MoveCommand { get; private set; }
        public MainViewModel(IWorld world, ICameraController cameraController)
        {
            _world = world;
            _cameraController = cameraController;
            _model3dGroup.Children.Add(_sphereGroup);
            _model3dGroup.Children.Add(_ropeGroup);
            Init3DPresentation();
            //InitBeam();
            AddSphere();

            CompositionTarget.Rendering += MoveSpheres;
            Task.Run(StartSimulation);
        }

        private void InitBeam()
        {
            if (_beam != null) _model3dGroup.Children.Remove(_beam);
            var brush = new SolidColorBrush(Colors.Black);
            var matGroup = new MaterialGroup();
            matGroup.Children.Add(new DiffuseMaterial(brush));
            matGroup.Children.Add(new SpecularMaterial(brush, 100));
            _beam = Models3D.CreateLine(start: _world.Origin,
                                           end: _world.Origin + (_world.Beam.Length * new Vector3D(1, 0, 0)),
                                           thickness: 10,
                                           brush: brush);
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 1, 0), _world.Beam.Angle)));
            transform.Children.Add(new TranslateTransform3D(_world.Beam.AnchorPoint - _world.Origin));
            _beam.Transform = transform;
            _model3dGroup.Children.Add(_beam);
        }

        private void AddSphere()
        {
            _world.AddSphere();
            foreach (Sphere sphereObj in _world.Spheres)
            {
                var brush = new SolidColorBrush(Colors.Crimson);
                var matGroup = new MaterialGroup();
                matGroup.Children.Add(new DiffuseMaterial(brush));
                matGroup.Children.Add(new SpecularMaterial(brush, 100));
                var sphere = Models3D.CreateSphere(matGroup);
                var transform = new Transform3DGroup();
                transform.Children.Add(new ScaleTransform3D(1, 1, 1));
                transform.Children.Add(new TranslateTransform3D(sphereObj.Position - _world.Origin));
                sphere.Transform = transform;
                _sphereGroup.Children.Add(sphere);
            }
            //foreach (Rope ropeObj in _world.Ropes)
            //{
            //    var brush = new SolidColorBrush(Colors.Black);
            //    var matGroup = new MaterialGroup();
            //    matGroup.Children.Add(new DiffuseMaterial(brush));
            //    matGroup.Children.Add(new SpecularMaterial(brush, 100));
            //    var rope = Models3D.CreateLine(start: _world.Origin,
            //                                   end: _world.Origin + (ropeObj.Length * new Vector3D(1, 0, 0)),
            //                                   thickness: 0.2f,
            //                                   brush: brush);
            //    var transform = new Transform3DGroup();
            //    transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), ropeObj.Angle)));
            //    transform.Children.Add(new TranslateTransform3D(ropeObj.AnchorPoint - _world.Origin));
            //    rope.Transform = transform;
            //    _model3dGroup.Children.Add(rope);
            //}
        }

        private Task StartSimulation()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            double previousTime = 0;

            while (true)
            {
                double deltaT = stopWatch.Elapsed.TotalSeconds - previousTime;
                previousTime = stopWatch.Elapsed.TotalSeconds;
                _world.UpdateSpheres(deltaT);
            }
        }

        private void MoveSpheres(object sender, EventArgs e)
        {
            for(int i = 0; i < _world.Spheres.Count; i++)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new ScaleTransform3D(1, 1, 1));
                transform.Children.Add(new TranslateTransform3D(_world.Spheres[i].Position - _world.Origin));
                _sphereGroup.Children[i].Transform = transform;
            }
        }

        #region Presentation setup


        private void Init3DPresentation()
        {
            SetupCamera();
            SetUpLights();
        }

        private void SetUpLights()
        {
            _model3dGroup.Children.Add(new AmbientLight(Colors.Gray));
            var direction = new Vector3D(1.5, -3, -5);
            _model3dGroup.Children.Add(new DirectionalLight(Colors.Gray, direction));
        }


        #endregion Presentation setup

        #region Camera control

        private void SetupCamera()
        {
            double l1 = (_world.Bounds.p1 - _world.Origin).Length;
            double l2 = (_world.Bounds.p2 - _world.Origin).Length;
            double radius = 2.3 * Math.Max(l1, l2);
            _cameraController.PositionCamera(radius, 0, 2.0 * Math.PI / 4);
        }

        public void ProcessKey(Key key)
        {
            _cameraController.ControlByKey(key);
        }

        public void Zoom(int Delta)
        {
            _cameraController.Zoom(Delta);
        }

        public void ControlByMouse(Vector vector)
        {
            _cameraController.ControlByMouse(vector);
        }


        #endregion Camera control
    }
}
