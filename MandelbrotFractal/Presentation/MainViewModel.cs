using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Presentation
{
    public class MainViewModel : ObservableObject
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
            DrawMandel();

        }

        private void CreateBitmap(int width, int height)
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
        }

        private void SetPixel(int row, int column, Color color)
        {
            uint intColor = BitConverter.ToUInt32(new byte[] { color.B, color.G, color.R, color.A });
            uint[] pixels = new uint[] { intColor };
            var rectangle = new Int32Rect(0, 0, 1, 1);
            BitmapDisplay.WritePixels(rectangle, pixels, BitmapDisplay.BackBufferStride, column, row);
        }

        private void DrawMandel()
        {
            for (int X = 0; X < maxRow; X++)
            {
                for (int Y = 0; Y < maxColumn; Y++)
                {
                    int init = logic.MandelbrotFractal(X, Y);
                    byte colorValue = (byte)((double)init / 100d * 255d);
                    SetPixel(X, Y, Color.FromRgb(colorValue, colorValue, colorValue));
                }
            }
        }
    }
}
