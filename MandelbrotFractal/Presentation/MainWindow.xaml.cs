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

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.E)
            {
                viewModel.ZoomInCommand.Execute(null);
            }
            else if (e.Key == Key.A)
            {
                viewModel.ZoomOutCommand.Execute(null);
            }
            else if (e.Key == Key.D)
            {
                viewModel.OffsetRightCommand.Execute(null);
            }
            else if (e.Key == Key.Q)
            {
                viewModel.OffsetLeftCommand.Execute(null);
            }
            else if (e.Key == Key.Z)
            {
                viewModel.OffsetUpCommand.Execute(null);
            }
            else if (e.Key == Key.S)
            {
                viewModel.OffsetDownCommand.Execute(null);
            }
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
        private void WindowPreviesMouseMove(object sende, MouseEventArgs e)
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
            } else if(y < 0) {
                y = 0;
            }
            Point mousePos = new Point(x, y);
            viewModel.MouseChangedCommand.Execute(mousePos);
            
        }
    }
}
