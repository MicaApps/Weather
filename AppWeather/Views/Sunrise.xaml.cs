using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AppWeather.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class Sunrise : Page
    {
        public Sunrise()
        {
            this.InitializeComponent();

        }

        private void Slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            UpdatePosition(e.NewValue);
        }

        private void UpdatePosition(double progress)
        {
            var x = progress;
            var y = (Math.Sin(2 * Math.PI * x + Math.PI / 2) + 1) / 8;

            Canvas.SetLeft(Point, x * (Window.Current.Bounds.Width / 2));
            Canvas.SetTop(Point, y * 300);
        }

    }
}
