using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
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

        public MainViewModel(IWorld world, ICameraController cameraController)
        {
            _world = world;
            _cameraController = cameraController;
        }

        #region Camera control

        private void SetupCamera()
        {
            double l1 = (_world.Bounds.p1 - _world.Origin).Length;
            double l2 = (_world.Bounds.p2 - _world.Origin).Length;
            double radius = 2.3 * Math.Max(l1, l2);
            _cameraController.PositionCamera(radius, Math.PI / 10, 2.0 * Math.PI / 5);
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
