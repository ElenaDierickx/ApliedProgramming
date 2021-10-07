using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
            var x = Math.Floor(e.GetPosition(this.bitmapArea).X * this.bitmapArea.Source.Width / this.bitmapArea.ActualWidth);
            var y = Math.Floor(e.GetPosition(this.bitmapArea).Y * this.bitmapArea.Source.Height / this.bitmapArea.ActualHeight);
            if (x > 799)
            {
                x = 799;
            } else if (x < 0)
            {
                x = 0;
            }
            if (y > 599)
            {
                y = 599;
            } else if(y < 0 ) {
                y = 0;
            }
            Point mousePos = new Point(x, y);
            if(e.LeftButton == MouseButtonState.Pressed && pressed == false && x < 800 && x > 0 && y > 0 && y < 600)
            {
                oldX = x;
                oldY = y;
                pressed = true;
            }
            if(e.LeftButton == MouseButtonState.Released && pressed && x < 800 && x > 0 && y > 0 && y < 600)
            {
                Point moved = new Point(x - oldX, y - oldY);
                pressed = false;                
                viewModel.PanningCommand.Execute(moved);
            }
            viewModel.MouseChangedCommand.Execute(mousePos);
        }


        private void ChangeSize(object sender, SizeChangedEventArgs e)
        {
            double[] size = new double[2] { e.NewSize.Width, e.NewSize.Height };
            viewModel.ResizeCommand.Execute(size);
        }
    }
}
