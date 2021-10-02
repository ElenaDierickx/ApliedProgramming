using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
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

        public string timeElapsed;

        public string TimeElapsed
        {
            get
            {
                return timeElapsed;
            }
            set
            {
                timeElapsed = value;
                OnPropertyChanged("TimeElapsed");
            }
        }

        public double scrollValue;
        public double ScrollValue
        {
            get
            {
                return scrollValue;
            }
            set
            {
                scrollValue = value;
                OnPropertyChanged("ScrollValue");
            }
        }

        public Point mousePosition;
        public Point MousePosition
        {
            get
            {
                return mousePosition;
            }
            set
            {
                mousePosition = value;
                OnPropertyChanged("MousePosition");
            }
        }

        public WriteableBitmap BitmapDisplay { get; private set; }

        public IRelayCommand ResetCommand { get; private set; }

        public IRelayCommand ZoomInCommand { get; private set; }
        public IRelayCommand ZoomOutCommand { get; private set; }

        public IRelayCommand OffsetRightCommand { get; private set; }
        public IRelayCommand OffsetLeftCommand { get; private set; }
        public IRelayCommand OffsetUpCommand { get; private set; }
        public IRelayCommand OffsetDownCommand { get; private set; }
        public IRelayCommand MouseChangedCommand { get; private set; }

        public int Iterations { get; set; }

        private double zoom = 1;
        private int offsetX = 0;
        private int offsetY = 0;

       

        public MainViewModel(ILogic logic)
        {
            this.logic = logic;
            Iterations = 20;
            ResetCommand = new RelayCommand(ResetMandel);
            ZoomInCommand = new RelayCommand(ZoomInMandel);
            ZoomOutCommand = new RelayCommand(ZoomOutMandel);
            OffsetRightCommand = new RelayCommand(OffsetMandelRight);
            OffsetLeftCommand = new RelayCommand(OffsetMandelLeft);
            OffsetUpCommand = new RelayCommand(OffsetMandelUp);
            OffsetDownCommand = new RelayCommand(OffsetMandelDown);
            MouseChangedCommand = new RelayCommand<Point>(MouseChanged);
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
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var list = logic.MandelbrotFractal(maxRow, maxColumn, Iterations, zoom, offsetX, offsetY);
            foreach(MandelPoint point in list)
            {
                byte colorValue = (byte)(point.iter / Iterations * 255d);
                SetPixel(point.X, point.Y, Color.FromRgb(colorValue, colorValue, colorValue));
            }
            stopWatch.Stop();
            TimeElapsed = stopWatch.ElapsedMilliseconds.ToString();
        }

        private readonly int zoomFactor = 2;
        private readonly int offsetFactor = 400;

        private void ZoomInMandel()
        {
            zoom += zoomFactor;
            DrawMandel();
        }

        private void ZoomOutMandel()
        {
            zoom -= zoomFactor;
            DrawMandel();
        }

        private void OffsetMandelRight()
        {
            offsetX += offsetFactor;
            DrawMandel();
        }

        private void OffsetMandelLeft()
        {
            offsetX -= offsetFactor;
            DrawMandel();
        }

        private void OffsetMandelUp()
        {
            offsetY -= offsetFactor;
            DrawMandel();
        }

        private void OffsetMandelDown()
        {
            offsetY += offsetFactor;
            DrawMandel();
        }

        private void MouseChanged(Point point)
        {
            MousePosition = point;
        }

        private void ResetMandel()
        {
            offsetX = 0;
            offsetY = 0;
            zoom = 1;
            DrawMandel();
        }
    }
}
