using AppWeather.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Graphics.Canvas.Geometry;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
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
        public HomePageViewModel ViewModel => (HomePageViewModel)DataContext;

        public HomePage()
        {
            this.InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<HomePageViewModel>();

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

        }

        private void Canvas_Draw(Microsoft.Graphics.Canvas.UI.Xaml.CanvasControl sender, Microsoft.Graphics.Canvas.UI.Xaml.CanvasDrawEventArgs args)
        {
            //using (var canvasDrawingSession = args.DrawingSession)
            //{
            //    //canvasDrawingSession.DrawGeometry
            //    canvasDrawingSession.DrawText("lindexi", new Vector2(100, 100), Color.FromArgb(0xFF, 100, 100, 100));
            //}

            using (var canvasPathBuilder = new CanvasPathBuilder(args.DrawingSession))
            {
                // 这里可以画出 Path 或写出文字 lindexi.github.io
                //canvasPathBuilder.BeginFigure(100, 100);
                //int dx = 1, dy = 30;
                //canvasPathBuilder.AddLine(200, 100);
                double scaleX = sender.ActualWidth / (2 * Math.PI); // X轴的缩放因子
                double scaleY = sender.ActualHeight / 2; // Y轴的缩放因子
                canvasPathBuilder.BeginFigure((float)-Math.PI, 0);

                for (double x = -Math.PI; x <= Math.PI; x += 0.01)
                {
                    double y = Math.Cos(x);
                    canvasPathBuilder.AddLine(new Vector2(float.Parse((scaleX * (x + Math.PI)).ToString()), float.Parse((scaleY * (1 - y)).ToString())));
                }
                //int x0 = 360;
                //double y0 = Math.Sin(-x0 * Math.PI / 180.0);
                //canvasPathBuilder.BeginFigure(new Vector2(-x0, (float)(dy * y0)));
                //canvasPathBuilder.BeginFigure(100, 100);
                //for (int x = -x0; x < x0; x += dx)
                //{
                //    double y = Math.Sin(x * Math.PI / 180.0);
                //    canvasPathBuilder.AddLine(new Vector2(x, (float)(dy * y)));
                //}

                canvasPathBuilder.EndFigure(CanvasFigureLoop.Open);

                args.DrawingSession.DrawGeometry(CanvasGeometry.CreatePath(canvasPathBuilder), Colors.Gray, 2);
            }


        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Canvas.RemoveFromVisualTree();
            Canvas = null;
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


}
