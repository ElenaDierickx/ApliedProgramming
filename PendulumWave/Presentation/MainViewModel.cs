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
        private readonly ICameraController _cameraController2;

        private readonly SolidColorBrush[] _colorBrushList = new SolidColorBrush[]
     {
            new SolidColorBrush(Colors.Crimson),
            new SolidColorBrush(Colors.MediumBlue),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.DarkOrange),
            new SolidColorBrush(Colors.Olive),
            new SolidColorBrush(Colors.DarkCyan),
            new SolidColorBrush(Colors.Brown),
            new SolidColorBrush(Colors.SteelBlue),
            new SolidColorBrush(Colors.Gold),
            new SolidColorBrush(Colors.MistyRose),
            new SolidColorBrush(Colors.PaleTurquoise),
            new SolidColorBrush(Colors.PeachPuff),
            new SolidColorBrush(Colors.Salmon),
            new SolidColorBrush(Colors.Silver),
     };

        private int seconds;
        public int Seconds
        {
            get
            {
                return seconds;
            }
            set
            {
                seconds = value;
                OnPropertyChanged("Seconds");
            }
        }

        private readonly Model3DGroup _model3dGroup = new();
        public ProjectionCamera Camera => _cameraController.Camera;
        public ProjectionCamera Camera2 => _cameraController2.Camera;
        public Model3D Visual3dContent => _model3dGroup;
        private readonly Model3DGroup _sphereGroup = new();

        private readonly Model3DGroup _ropeGroup = new();
        private GeometryModel3D _beam;

        public IRelayCommand PauseCommand { get; private set; }
        public IRelayCommand ResetCommand { get; private set; }
        public IRelayCommand ChangePendulumAmount { get; private set; }
        public IRelayCommand ChangeColorCommand { get; private set; }

        private int pendulumAmount = 10;

        public MainViewModel(IWorld world, ICameraController cameraController, ICameraController cameraController2)
        {
            _world = world;
            _cameraController = cameraController;
            _cameraController2 = cameraController2;
            _model3dGroup.Children.Add(_sphereGroup);
            _model3dGroup.Children.Add(_ropeGroup);
            Init3DPresentation();

            PauseCommand = new RelayCommand(PausePendulum);
            ResetCommand = new RelayCommand(ResetPendulum);
            ChangePendulumAmount = new RelayCommand<int>(PendulumAmountChanged);
            ChangeColorCommand = new RelayCommand<bool>(ChangeColor);

            CompositionTarget.Rendering += MovePendulumRopes;
            

            AddPendulumRope();
            Task.Run(StartSimulation);
        }

        private void PendulumAmountChanged(int amount)
        {
            pendulumAmount = amount;
            ResetPendulum();
        }

        private bool color = false;

        private void ChangeColor(bool isChecked)
        {
            color = isChecked;
            _sphereGroup.Children.Clear();
            foreach (Rope ropeObj in _world.Ropes)
            {
                AddSphere(ropeObj);
            }
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
            reset = true;
            AddPendulumRope();
            Task.Run(StartSimulation);
        }

        private void AddPendulumRope()
        {
            _world.AddPendulumRope(pendulumAmount);
            foreach (Rope ropeObj in _world.Ropes)
            {
                AddRope(ropeObj);
                AddSphere(ropeObj);
            }
        }

        private void AddRope(Rope ropeObj)
        {
            var brush = new SolidColorBrush(Colors.Black);
            var matGroup = new MaterialGroup();
            matGroup.Children.Add(new DiffuseMaterial(brush));
            matGroup.Children.Add(new SpecularMaterial(brush, 100));
            var rope = Models3D.CreateLine(start: _world.Origin,
                                           end: _world.Origin - (ropeObj.Length * new Vector3D(1, 0, 0)),
                                           thickness: 0.005f,
                                           brush: brush);
            var transform = new Transform3DGroup();
            transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + ropeObj.Angle)));
            transform.Children.Add(new TranslateTransform3D(ropeObj.AnchorPoint - _world.Origin));
            rope.Transform = transform;
            _ropeGroup.Children.Add(rope);
        }

        private void AddSphere(Rope ropeObj)
        {
            SolidColorBrush brush;
            if (color)
            {
                brush = _colorBrushList[_world.Ropes.IndexOf(ropeObj) % _colorBrushList.Length];
            }
            else
            {
                brush = new SolidColorBrush(Colors.Crimson);
            }
            var matGroup = new MaterialGroup();
            matGroup.Children.Add(new DiffuseMaterial(brush));
            matGroup.Children.Add(new SpecularMaterial(brush, 100));
            var sphere = Models3D.CreateSphere(matGroup);
            var transform = new Transform3DGroup();
            Point3D spherePos = new() { X = ropeObj.AnchorPoint.X + ropeObj.Length * Math.Cos((ropeObj.Angle - 90) * (Math.PI / 180)), Y = ropeObj.AnchorPoint.Y + ropeObj.Length * Math.Sin((ropeObj.Angle - 90) * (Math.PI / 180)), Z = ropeObj.AnchorPoint.Z };
            transform.Children.Add(new ScaleTransform3D(0.01f, 0.01f, 0.01f));
            transform.Children.Add(new TranslateTransform3D(spherePos - _world.Origin));
            sphere.Transform = transform;
            _sphereGroup.Children.Add(sphere);
        }

        private bool pausePressed = false;
        private bool reset = false;
        private bool paused = false;

        private void StartSimulation()
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            double previousTime = 0;

            while (!reset)
            {
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
                Seconds = stopWatch.Elapsed.Seconds;
                
                _world.UpdatePendulumRopes(deltaT);
            }

            reset = false;
        }

        private void MovePendulumRopes(object sender, EventArgs e)
        {
            for (int i = 0; i < _world.Ropes.Count; i++)
            {
                if (!paused)
                {
                    var transform = new Transform3DGroup();
                    transform.Children.Add(new RotateTransform3D(new AxisAngleRotation3D(new Vector3D(0, 0, 1), 90 + _world.Ropes[i].Angle)));
                    transform.Children.Add(new TranslateTransform3D(_world.Ropes[i].AnchorPoint - _world.Origin));
                    _ropeGroup.Children[i].Transform = transform;

                    transform = new Transform3DGroup();
                    Point3D spherePos = new() { X = _world.Ropes[i].AnchorPoint.X + _world.Ropes[i].Length * Math.Cos((_world.Ropes[i].Angle - 90) * (Math.PI / 180)), Y = _world.Ropes[i].AnchorPoint.Y + _world.Ropes[i].Length * Math.Sin((_world.Ropes[i].Angle - 90) * (Math.PI / 180)), Z = _world.Ropes[i].AnchorPoint.Z };
                    transform.Children.Add(new ScaleTransform3D(0.01f, 0.01f, 0.01f));
                    transform.Children.Add(new TranslateTransform3D(spherePos - _world.Origin));
                    _sphereGroup.Children[i].Transform = transform;
                }
            }
        }

        #region Presentation setup


        private void Init3DPresentation()
        {
            SetupCamera();
            SetupCamera2();
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

        private void SetupCamera2()
        {
            double l1 = (_world.Bounds.p1 - _world.Origin).Length;
            double l2 = (_world.Bounds.p2 - _world.Origin).Length;
            double radius = 2.3 * Math.Max(l1, l2);
            _cameraController2.PositionCamera(radius, 0, Math.PI);
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
