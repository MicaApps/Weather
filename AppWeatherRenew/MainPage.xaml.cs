using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AppCoreOld2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
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
                appCallbacks.RenderingStarted += () =>
                {
                    m_DXSwapChainPanel.Children.Remove(m_ExtendedSplashGrid);
                    if (onResizeHandler != null)
                    {
                        Window.Current.SizeChanged -= onResizeHandler;
                        onResizeHandler = null;
                    }
                };

                //if (Windows.Foundation.Metadata.ApiInformation.IsApiContractPresent("Windows.Phone.PhoneContract", 1))
                //    isPhone = true;

                appCallbacks.SetSwapChainPanel(m_DXSwapChainPanel);
                appCallbacks.SetCoreWindowEvents(Window.Current.CoreWindow);
                appCallbacks.InitializeD3DXAML();

                splashScreen = ((App)App.Current).splashScreen;
                GetSplashBackgroundColor();
                OnResize();
                onResizeHandler = new WindowSizeChangedEventHandler((o, e) => OnResize());
                Window.Current.SizeChanged += onResizeHandler;
            }
        }
        private SplashScreen splashScreen;
        private Rect splashImageRect;
        private WindowSizeChangedEventHandler onResizeHandler;

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
    }
}
