using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Presentation
{
    class MainViewModel : ObservableObject
    {
        private const int maxRow = 600;
        private const int maxColumn = 800;
        private readonly ILogic logic;
        public string Title => "Mandelbrot Fractal";

        public string Background { get; private set; }

        public WriteableBitmap BitmapDisplay { get; private set; }

        public MainViewModel(ILogic logic)
        {
            this.logic = logic;
            CreateBitmap(maxColumn, maxRow);
        }

        private void CreateBitmap(int width, int height)
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
        }
    }
}
