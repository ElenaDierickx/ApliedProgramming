using LogicLayer;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows;
using System.Threading;

namespace Presentation
{
    public class MainViewModel : ObservableObject
    {
        private int maxRow = 600;
        private int maxColumn = 800;
        
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

        private DoublePoint cornerPosition;
        public DoublePoint CornerPosition
        {
            get
            {
                return cornerPosition;
            }
            set
            {
                cornerPosition = value;
                OnPropertyChanged("CornerPosition");
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
        public IRelayCommand ResizeCommand { get; private set; }

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
            ResizeCommand = new RelayCommand<double[]>(Resize);
            CreateBitmap();
            Iterations = new List<int> { 25, 100, 150, 200, 250, 500, 750, 1000, 2000, 5000, 10000 };
            Iteration = 1000;
            ColorSchemes = new List<string> { "GreyScale", "Banding", "UglyBanding", "Colors" };
            ColorScheme = "Colors";
            CornerPosition = logic.Scaling(0, maxRow, maxRow, maxColumn, Zoom, offsetX, offsetY);
        }

        private void CreateBitmap()
        {
            double dpiX = 96d;
            double dpiY = 96d;
            var pixelFormat = PixelFormats.Pbgra32;
            BitmapDisplay = new WriteableBitmap(maxColumn, maxRow, dpiX, dpiY, pixelFormat, null);
            OnPropertyChanged(nameof(BitmapDisplay));
        }

        private async void Resize(double[] size)
        {
            maxRow = (int)size[1];
            maxColumn = (int)size[0];
            CreateBitmap();
            await DrawMandel();
        }

        private async Task SetPixels(CancellationTokenSource tokenSource)
        {
            int[,] colorInts = new int[maxRow,maxColumn];
            await Task.Run(() =>
            {
                
                switch (ColorScheme)
                {
                    case "GreyScale":
                        colorInts = logic.GreyScale(maxRow, maxColumn, mandelPoints, Iteration, tokenSource.Token);
                        break;
                    case "Banding":
                        colorInts = logic.Banding(maxRow, maxColumn, mandelPoints, tokenSource.Token);
                        break;
                    case "UglyBanding":
                        colorInts = logic.UglyBanding(maxRow, maxColumn, mandelPoints, tokenSource.Token);
                        break;
                    case "Colors":
                        colorInts = logic.Colors(maxRow, maxColumn, mandelPoints, Iteration, tokenSource.Token);
                        break;
                    default:
                        colorInts = logic.GreyScale(maxRow, maxColumn, mandelPoints, Iteration, tokenSource.Token);
                        break;
                }
            });
            if(colorInts.GetUpperBound(0) + 1 == maxRow && colorInts.GetUpperBound(1) + 1 == maxColumn && !tokenSource.Token.IsCancellationRequested)
            {
                var rectangle = new Int32Rect(0, 0, maxColumn, maxRow);
                BitmapDisplay.WritePixels(rectangle, colorInts, BitmapDisplay.BackBufferStride, 0, 0);
            }
            
        }

        private CancellationTokenSource tokenSource;

        private async Task DrawMandel()
        {
            if(tokenSource != null && !tokenSource.IsCancellationRequested)
            {
                tokenSource.Cancel();
            }

            tokenSource = new CancellationTokenSource();

            await Task.Run(() =>
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                mandelPoints = logic.MandelbrotFractal(maxRow, maxColumn, Iteration, Zoom, offsetX, offsetY, tokenSource.Token);
                stopWatch.Stop();
                TimeElapsed = stopWatch.ElapsedMilliseconds.ToString();
            });
            await SetPixels(tokenSource);
        }
        private readonly double zoomFactor = 2;

        private async void ZoomInMandel()
        {
            Zoom *= zoomFactor;
            CornerPosition = logic.Scaling(0, maxRow, maxRow, maxColumn, Zoom, offsetX, offsetY);
            await DrawMandel();
        }

        private async void ZoomOutMandel()
        {
            if (Zoom > 1)
            {
                Zoom /= zoomFactor;
                CornerPosition = logic.Scaling(0, maxRow, maxRow, maxColumn, Zoom, offsetX, offsetY);
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
            CornerPosition = logic.Scaling(0, maxRow, maxRow, maxColumn, Zoom, offsetX, offsetY);
            await DrawMandel();
        }

        private async void Panning(Point moved)
        {
            offsetX -= moved.X / maxColumn / Zoom;
            offsetY -= moved.Y / maxRow / Zoom;
            CornerPosition = logic.Scaling(0, maxRow, maxRow, maxColumn, Zoom, offsetX, offsetY);
            await DrawMandel();
        }
    }
}
