using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
        public MainWindow(MainViewModel vm)
        {
            DataContext = vm;
            InitializeComponent();
        }

        private void WindowPreviewKeyDown(object sender, KeyEventArgs e)
        {
            var viewModel = DataContext as MainViewModel;
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
    }
}
