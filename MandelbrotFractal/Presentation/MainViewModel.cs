using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private const int maxRow = 600;
        private const int maxColumn = 800;
        
        private readonly ILogic logic;
        public string Title => "Mandelbrot Fractal";

        public List<int> Iterations { get; set; }
        private int iteration;
        public int Iteration
        {
            get { return iteration; }
            set
            {
                if (iteration != value)
                {
                    iteration = value;
                    OnPropertyChanged("Iteration");
                    DrawMandel();
                }
            }
        }


        public List<string> ColorSchemes { get; set; }
        private string colorScheme;
        public string ColorScheme
        {
            get { return colorScheme; }
            set
            {
                if (colorScheme != value)
                {
                    colorScheme = value;
                    OnPropertyChanged("ColorScheme");
                    DrawMandel();
                }
            }
        }

        private string timeElapsed;

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

        private double scrollValue;
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

        private DoublePoint mousePosition;
        public DoublePoint MousePosition
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

        private int[,] mandelPoints;

        private int iterationPoint;
        public int IterationPoint
        {
            get
            {
                return iterationPoint;
            }
            set
            {
                iterationPoint = value;
                OnPropertyChanged("IterationPoint");
            }
        }

        public WriteableBitmap BitmapDisplay { get; private set; }

        public IRelayCommand ResetCommand { get; private set; }

        public IRelayCommand ZoomInCommand { get; private set; }
        public IRelayCommand ZoomOutCommand { get; private set; }
        public IRelayCommand MouseChangedCommand { get; private set; }
        public IRelayCommand PanningCommand { get; private set; }
        public IRelayCommand DrawMandelCommand { get; private set; }

        private double zoom = 1;
        public double Zoom
        {
            get
            {
                return zoom;
            }
            set
            {
                zoom = value;
                OnPropertyChanged("Zoom");
            }
        }

        private double offsetX = 0;
        private double offsetY = 0;
       

        public MainViewModel(ILogic logic)
        {
            this.logic = logic;
            ResetCommand = new RelayCommand(ResetMandel);
            ZoomInCommand = new RelayCommand(ZoomInMandel);
            ZoomOutCommand = new RelayCommand(ZoomOutMandel);
            MouseChangedCommand = new RelayCommand<Point>(MouseChanged);
            PanningCommand = new RelayCommand<Point>(Panning);
            CreateBitmap(maxColumn, maxRow);
            Iterations = new List<int> { 5, 10, 25, 100, 150, 200, 250, 500, 750, 1000 };
            Iteration = 200;
            ColorSchemes = new List<string> { "GreyScale", "Banding", "UglyBanding" };
            ColorScheme = "GreyScale";
        }

        private void CreateBitmap(int width, int height)
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(width, height, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
        }

        private void SetPixels()
        {
            int[,] colorInts;
            switch (ColorScheme){
                case "GreyScale":
                    colorInts = logic.GreyScale(maxRow, maxColumn, mandelPoints, Iteration);
                    break;
                case "Banding":
                    colorInts = logic.Banding(maxRow, maxColumn, mandelPoints);
                    break;
                case "UglyBanding":
                    colorInts = logic.UglyBanding(maxRow, maxColumn, mandelPoints);
                    break;
                default:
                    colorInts = logic.GreyScale(maxRow, maxColumn, mandelPoints, Iteration);
                    break;
            }
            var rectangle = new Int32Rect(0, 0, maxColumn, maxRow);
            BitmapDisplay.WritePixels(rectangle, colorInts, BitmapDisplay.BackBufferStride, 0, 0);
        }

        

        private async Task DrawMandel()
        {
            await Task.Run(() =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                mandelPoints = logic.MandelbrotFractal(maxRow, maxColumn, Iteration, Zoom, offsetX, offsetY);
                stopWatch.Stop();
                TimeElapsed = stopWatch.ElapsedMilliseconds.ToString();
            });
            SetPixels();
        }

        private readonly double zoomFactor = 2;

        private async void ZoomInMandel()
        {
            Zoom *= zoomFactor;
            await DrawMandel();
        }

        private async void ZoomOutMandel()
        {
            if (Zoom > 1)
            {
                Zoom /= zoomFactor;
                await DrawMandel();
            }
        }

        private void MouseChanged(Point point)
        {
            MousePosition = logic.Scaling((int)point.X, (int)point.Y, maxRow, maxColumn, Zoom, offsetX, offsetY);
            IterationPoint = mandelPoints[(int)point.Y, (int)point.X];
        }

        private async void ResetMandel()
        {
            offsetX = 0;
            offsetY = 0;
            Zoom = 1;
            await DrawMandel();
        }

        private async void Panning(Point moved)
        {
            offsetX -= moved.X / maxColumn / Zoom;
            offsetY -= moved.Y / maxRow / Zoom;
            await DrawMandel();
        }
    }
}
