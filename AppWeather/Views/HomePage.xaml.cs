using AppWeather.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Geometry;
using Microsoft.Graphics.Canvas.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Web.WebView2.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using UnityPlayer;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AppWeather.Views
{
    public sealed partial class HomePage : Page
    {
        private SplashScreen splashScreen;
        private Rect splashImageRect;
        private WindowSizeChangedEventHandler onResizeHandler;
        private bool isPhone = false;

        public HomePageViewModel ViewModel => (HomePageViewModel)DataContext;
        public HomePage()
        {
            this.InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<HomePageViewModel>();
            //ViewModel.PropertyChanged += ViewModel_PropertyChanged;
            ViewModel.Initialize();
            Canvas.Invalidate();


            var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
            var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            String theme = localSettings.Values["Theme"] as string;
            if (theme != null)
            {
                if (theme.Equals("Dark"))
                {
                    uiTheme = "#FF000000";
                }
                else if (theme.Equals("Light"))
                {
                    uiTheme = "#FFFFFFFF";
                }
            }

            if (uiTheme == "#FF000000")
            {
                //dark
                TempLine.DataPointStyle = this.Resources["DarkModeAreaDataPointStyle"] as Style;
            }
            else if (uiTheme == "#FFFFFFFF")
            {
                //light
                TempLine.DataPointStyle = (Style)this.Resources["LightModeAreaDataPointStyle"] as Style;
            }

            //SunriseFrame.Navigate(typeof(Sunrise));

            NavigationCacheMode = Windows.UI.Xaml.Navigation.NavigationCacheMode.Required;

            AppCallbacks appCallbacks = AppCallbacks.Instance;

            bool isWindowsHolographic = false;

#if UNITY_HOLOGRAPHIC
            // If application was exported as Holographic check if the device actually supports it,
            // otherwise we treat this as a normal XAML application
            isWindowsHolographic = AppCallbacks.IsMixedRealitySupported();
#endif

            if (isWindowsHolographic)
            {
                appCallbacks.InitializeViewManager(Window.Current.CoreWindow);
            }
            else
            {
                appCallbacks.RenderingStarted += () => { RemoveSplashScreen(); };

                if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1))
                    isPhone = true;

                appCallbacks.SetSwapChainPanel(GetSwapChainPanel());
                appCallbacks.SetCoreWindowEvents(Window.Current.CoreWindow);
                appCallbacks.InitializeD3DXAML();

                splashScreen = ((App)App.Current).splashScreen;
                GetSplashBackgroundColor();
                OnResize();
                onResizeHandler = new WindowSizeChangedEventHandler((o, e) => OnResize());
                Window.Current.SizeChanged += onResizeHandler;
            }
        }
        private void OnResize()
        {
            if (splashScreen != null)
            {
                splashImageRect = splashScreen.ImageLocation;
                PositionImage();
            }
        }

        private void PositionImage()
        {
            var inverseScaleX = 1.0f;
            var inverseScaleY = 1.0f;
            if (isPhone)
            {
                inverseScaleX = inverseScaleX / m_DXSwapChainPanel.CompositionScaleX;
                inverseScaleY = inverseScaleY / m_DXSwapChainPanel.CompositionScaleY;
            }

            m_ExtendedSplashGrid.SetValue(Windows.UI.Xaml.Controls.Canvas.LeftProperty, splashImageRect.X * inverseScaleX);
            m_ExtendedSplashGrid.SetValue(Windows.UI.Xaml.Controls.Canvas.TopProperty, splashImageRect.Y * inverseScaleY);
            m_ExtendedSplashGrid.Height = splashImageRect.Height * inverseScaleY;
            m_ExtendedSplashGrid.Width = splashImageRect.Width * inverseScaleX;
        }

        private async void GetSplashBackgroundColor()
        {
            //wvWindDegree.cor
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///AppxManifest.xml"));
                string manifest = await FileIO.ReadTextAsync(file);
                int idx = manifest.IndexOf("SplashScreen");
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("BackgroundColor");
                if (idx < 0)  // background is optional
                    return;
                manifest = manifest.Substring(idx);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(idx + 1);
                idx = manifest.IndexOf("\"");
                manifest = manifest.Substring(0, idx);
                int value = 0;
                bool transparent = false;
                if (manifest.Equals("transparent"))
                    transparent = true;
                else if (manifest[0] == '#') // color value starts with #
                    value = Convert.ToInt32(manifest.Substring(1), 16) & 0x00FFFFFF;
                else
                    return; // at this point the value is 'red', 'blue' or similar, Unity does not set such, so it's up to user to fix here as well
                byte r = (byte)(value >> 16);
                byte g = (byte)((value & 0x0000FF00) >> 8);
                byte b = (byte)(value & 0x000000FF);

                await CoreWindow.GetForCurrentThread().Dispatcher.RunAsync(CoreDispatcherPriority.High, delegate ()
                {
                    byte a = (byte)(transparent ? 0x00 : 0xFF);
                    m_ExtendedSplashGrid.Background = new SolidColorBrush(Color.FromArgb(a, r, g, b));
                });
            }
            catch (Exception)
            { }
        }
        public SwapChainPanel GetSwapChainPanel()
        {
            return m_DXSwapChainPanel;
        }

        public void RemoveSplashScreen()
        {
            m_DXSwapChainPanel.Children.Remove(m_ExtendedSplashGrid);
            if (onResizeHandler != null)
            {
                Window.Current.SizeChanged -= onResizeHandler;
                onResizeHandler = null;
            }
        }
        private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Sunrise" || e.PropertyName == "Sunset")
            {
                // Redraw the graph
                OnDraw();
            }
        }
        private CanvasRenderTarget renderTarget;
        void OnDraw()
        {
        }

        public TimeSpan ParseTimeSpan(string timeString)
        {
            TimeSpan timeSpan;
            if (!TimeSpan.TryParse(timeString, out timeSpan))
            {
                throw new FormatException($"Invalid time format: {timeString}");
            }
            return timeSpan;                                                                                                                              
        }

        private void InitializeWebviews()
        {
            var baseUri = "ms-appx:///Resources/WebPages";
            _ = loadWebView2(WebView_WindDegree, new Uri(baseUri + "/Wind.html"));
            _ = loadWebView2(WebView_Pressure, new Uri(baseUri + "/Pressure.html"));
                                                                                              
        }


        private static async Task<StorageFile> loadWebView2(WebView2 webview2, Uri uri)
        {
            var storageFile = await StorageFile.GetFileFromApplicationUriAsync(uri);
            webview2.Source = new Uri(storageFile.Path);
            return storageFile;
        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            //using (var canvasDrawingSession = args.DrawingSession)
            //{
            //    //canvasDrawingSession.DrawGeometry
            //    canvasDrawingSession.DrawText("lindexi", new Vector2(100, 100), Color.FromArgb(0xFF, 100, 100, 100));
            //}
            // 这里可以画出 Path 或写出文字 lindexi.github.io
            //canvasPathBuilder.BeginFigure(100, 100);
            //int dx = 1, dy = 30;
            //canvasPathBuilder.AddLine(200, 100);

            //int x0 = 360;
            //double y0 = Math.Sin(-x0 * Math.PI / 180.0);
            //canvasPathBuilder.BeginFigure(new Vector2(-x0, (float)(dy * y0)));
            //canvasPathBuilder.BeginFigure(100, 100);
            //for (int x = -x0; x < x0; x += dx)
            //{
            //    double y = Math.Sin(x * Math.PI / 180.0);
            //    canvasPathBuilder.AddLine(new Vector2(x, (float)(dy * y)));
            //}
            Thread.Sleep(100);
            double scaleX = sender.ActualWidth / (2 * Math.PI); // X轴的缩放因子
            double scaleY = sender.ActualHeight / 2; // Y轴的缩放因子
            using (var canvasPathBuilder = new CanvasPathBuilder(args.DrawingSession))
            {
                canvasPathBuilder.BeginFigure((float)-Math.PI, 0);

                for (double x = -Math.PI; x <= Math.PI; x += 0.01)
                {
                    double y = Math.Cos(x);
                    canvasPathBuilder.AddLine(new Vector2(float.Parse((scaleX * (x + Math.PI)).ToString()), float.Parse((scaleY * (1 - y)).ToString())));
                }

                //canvasPathBuilder.BeginFigure(0, (float)scaleY);
                //canvasPathBuilder.AddLine(new Vector2((float)sender.ActualWidth, (float)scaleY));
                canvasPathBuilder.EndFigure(CanvasFigureLoop.Open);
                args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(canvasPathBuilder), Colors.Gray, 2);
            }
            using (var axisPathBuilder = new CanvasPathBuilder(args.DrawingSession))
            {
                // Draw the X-axis
                axisPathBuilder.BeginFigure(0, (float)scaleY);
                axisPathBuilder.AddLine(new Vector2((float)sender.ActualWidth, (float)scaleY));
                axisPathBuilder.EndFigure(CanvasFigureLoop.Open);

                args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(axisPathBuilder), Colors.Gray, 2);
            }
            var viewModel = this.DataContext as HomePageViewModel;
            TimeSpan time1 = string.IsNullOrEmpty(viewModel.Sunrise) ? TimeSpan.Zero : ParseTimeSpan(viewModel.Sunrise);
            TimeSpan time2 = string.IsNullOrEmpty(viewModel.Sunset) ? TimeSpan.Zero : ParseTimeSpan(viewModel.Sunset);
            double hours1 = time1.TotalMinutes / 60.0;
            double hours2 = time2.TotalMinutes / 60.0;
            // Convert the times to radians
            double radian1 = (hours1 / 24) * 2 * Math.PI - Math.PI;
            double radian2 = (hours2 / 24) * 2 * Math.PI - Math.PI;

            // Calculate the y values
            double y1 = Math.Cos(radian1);
            double y2 = Math.Cos(radian2);

            // Calculate the screen positions
            Vector2 pos1 = new Vector2((float)(scaleX * (radian1 + Math.PI)), (float)(scaleY * (1 - y1)));
            Vector2 pos2 = new Vector2((float)(scaleX * (radian2 + Math.PI)), (float)(scaleY * (1 - y2)));
            CanvasRenderTarget renderTarget = new CanvasRenderTarget(Canvas, (float)Canvas.ActualWidth, (float)Canvas.ActualHeight, 96);
            args.DrawingSession.FillCircle(pos1, 5, Colors.White);
            args.DrawingSession.FillCircle(pos2, 5, Colors.White);
        }

        //20240802由xiapeng01@126.com注释
        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //20240802由xiapeng01@126.com注释
            //Canvas.RemoveFromVisualTree();
            //Canvas = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeWebviews();
        }

        private void WebView_WindDegree_CoreWebView2Initialized(WebView2 sender, CoreWebView2InitializedEventArgs args)
        {
            sender.CoreWebView2.Environment.SetEnvironmentVariable("WEBVIEW2_DEFAULT_BACKGROUND_COLOR", "00FFFFFF");
        }

        //private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        //{
        //    UpdatePosition(e.NewValue);
        //}

        //private void UpdatePosition(double progress)
        //{
        //    var x = progress;
        //    var y = (Math.Sin(2 * Math.PI * x + Math.PI / 2) + 1) / 8;

        //    Canvas.SetLeft(Point, x * (Window.Current.Bounds.Width / 2));
        //    Canvas.SetTop(Point, y * 300);
        //}
    }

    public class PercentageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double actualHeight = (double)value;
            double percentage = System.Convert.ToDouble(parameter);
            return actualHeight * percentage;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }


    public class ValueConverter : IValueConverter
    {
        public UIElement UIParameter { get; set; }
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            double result = (double)value;
            var pointerValue = (80 - result);
            if (pointerValue <= 40)
            {
                pointerValue = 40 - pointerValue;
            }
            else
            {
                pointerValue = 40 + result;
            }

            return pointerValue;
        }

        // No need to implement converting back on a one-way binding
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return value;
        }
    }


    //public class TicksConverter : IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, string language)
    //    {
    //        double sliderValue = (double)value;
    //        return sliderValue;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, string language)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    //public class TicksSource : IEnumerable
    //{
    //    public double Minimum { get; set; }
    //    public double Maximum { get; set; }

    //    public IEnumerator GetEnumerator()
    //    {
    //        for (double i = Minimum; i <= Maximum; i += (Maximum - Minimum) / 10)
    //        {
    //            yield return i;
    //        }
    //    }
    //}

}
