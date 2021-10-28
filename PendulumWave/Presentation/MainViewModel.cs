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

        public IRelayCommand PauseCommand { get; private set; }
        public IRelayCommand ResetCommand { get; private set; }

        public MainViewModel(IWorld world, ICameraController cameraController)
        {
            _world = world;
            _cameraController = cameraController;
            _model3dGroup.Children.Add(_sphereGroup);
            _model3dGroup.Children.Add(_ropeGroup);
            Init3DPresentation();

            PauseCommand = new RelayCommand(PausePendulum);
            ResetCommand = new RelayCommand(ResetPendulum);

            CompositionTarget.Rendering += MovePendulumRopes;
            

            AddPendulumRope();
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

        private void PausePendulum()
        {
            pausePressed = true;
        }

        private void ResetPendulum()
        {
            _sphereGroup.Children.Clear();
            _ropeGroup.Children.Clear();
            AddPendulumRope();
            Task.Run(StartSimulation);
        }

        private void AddPendulumRope()
        {
            _world.AddPendulumRope(10);
            foreach (Rope ropeObj in _world.Ropes)
            {
                var brush = new SolidColorBrush(Colors.Black);
                var matGroup = new MaterialGroup();
                matGroup.Children.Add(new DiffuseMaterial(brush));
                matGroup.Children.Add(new SpecularMaterial(brush, 100));
                var rope = Models3D.CreateLine(start: _world.Origin,
                                               end: _world.Origin - (ropeObj.Length * new Vector3D(1, 0, 0)),
                                               thickness: 0.2f,
                                               brush: brush);
                var transform = new Transform3DGroup();
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + ropeObj.Angle)));
                transform.Children.Add(new TranslateTransform3D(ropeObj.AnchorPoint - _world.Origin));
                rope.Transform = transform;
                _ropeGroup.Children.Add(rope);

                brush = new SolidColorBrush(Colors.Crimson);
                matGroup = new MaterialGroup();
                matGroup.Children.Add(new DiffuseMaterial(brush));
                matGroup.Children.Add(new SpecularMaterial(brush, 100));
                var sphere = Models3D.CreateSphere(matGroup);
                transform = new Transform3DGroup();
                Point3D spherePos = new() { X = ropeObj.AnchorPoint.X + ropeObj.Length * Math.Cos(ropeObj.Angle * (Math.PI / 180)), Y = ropeObj.AnchorPoint.Y - ropeObj.Length * Math.Sin(ropeObj.Angle * (Math.PI / 180)), Z = ropeObj.AnchorPoint.Z };
                transform.Children.Add(new ScaleTransform3D(0.8f, 0.8f, 0.8f));
                transform.Children.Add(new TranslateTransform3D(spherePos - _world.Origin));
                sphere.Transform = transform;
                _sphereGroup.Children.Add(sphere);
            }
        }

        private bool pausePressed = false;
        private bool reset = false;

        private Task StartSimulation()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            double previousTime = 0;
            bool paused = false;

            while (true)
            {
                if (reset)
                {
                    reset = false;
                    return Task.CompletedTask;
                }
                if (pausePressed && paused)
                {
                    stopWatch.Start();
                    pausePressed = false;
                    paused = false;
                }
                if(pausePressed && !paused)
                {
                    stopWatch.Stop();
                    pausePressed = false;
                    paused = true;
                }
                double deltaT = stopWatch.Elapsed.TotalSeconds - previousTime;
                previousTime = stopWatch.Elapsed.TotalSeconds;
                _world.UpdatePendulumRopes(deltaT);
            }
        }

        private void MovePendulumRopes(object sender, EventArgs e)
        {
            for (int i = 0; i < _world.Ropes.Count; i++)
            {
                var transform = new Transform3DGroup();
                transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + _world.Ropes[i].Angle)));
                transform.Children.Add(new TranslateTransform3D(_world.Ropes[i].AnchorPoint - _world.Origin));
                _ropeGroup.Children[i].Transform = transform;

                transform = new Transform3DGroup();
                Point3D spherePos = new() { X = _world.Ropes[i].AnchorPoint.X + _world.Ropes[i].Length * Math.Cos((_world.Ropes[i].Angle - 90) * (Math.PI / 180)), Y = _world.Ropes[i].AnchorPoint.Y + _world.Ropes[i].Length * Math.Sin((_world.Ropes[i].Angle - 90) * (Math.PI / 180)), Z = _world.Ropes[i].AnchorPoint.Z };
                transform.Children.Add(new ScaleTransform3D(0.8f, 0.8f, 0.8f));
                transform.Children.Add(new TranslateTransform3D(spherePos - _world.Origin));
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
