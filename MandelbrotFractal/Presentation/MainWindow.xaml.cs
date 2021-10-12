using System;
using System.Windows;
using System.Windows.Input;

namespace Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel viewModel;

        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            viewModel = DataContext as MainViewModel;
            InitializeComponent();
            this.SizeChanged += ChangeSize;
        }

        private void Scroll(object sender, MouseWheelEventArgs e)
        {
            if(e.Delta > 0)
            {
                viewModel.ZoomInCommand.Execute(null);
            }
            else
            {
                viewModel.ZoomOutCommand.Execute(null);
            }
        }

        private bool pressed = false;
        private double oldX = 0;
        private double oldY = 0;
        private void WindowPreviesMouseMove(object sender, MouseEventArgs e)
        {
            var x = e.GetPosition(this.bitmapArea).X;
            var y = e.GetPosition(this.bitmapArea).Y;
            Point mousePos = new Point(x, y);
            if(e.LeftButton == MouseButtonState.Pressed && pressed == false)
            {
                oldX = x;
                oldY = y;
                pressed = true;
            }
            if(e.LeftButton == MouseButtonState.Released && pressed)
            {
                Point moved = new Point(x - oldX, y - oldY);
                pressed = false;                
                viewModel.PanningCommand.Execute(moved);
            }
            viewModel.MouseChangedCommand.Execute(mousePos);
        }

        private void ChangeSize(object sender, SizeChangedEventArgs e)
        {
            double bitmapWidth = e.NewSize.Width;
            double bitmapHeight = e.NewSize.Height;
            double[] size = new double[2] { bitmapWidth, bitmapHeight };
            viewModel.ResizeCommand.Execute(size);
        }
    }
}
