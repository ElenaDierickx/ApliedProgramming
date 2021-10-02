using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            if (x > 800)
            {
                x = 800;
            } else if (x < 0)
            {
                x = 0;
            }
            if (y > 600)
            {
                y = 600;
            } else if(y < 0 ) {
                y = 0;
            }
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
    }
}
