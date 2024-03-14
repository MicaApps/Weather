using AppWeather.Views;
using AppWeather.Views.Styles;
using System;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AppWeather
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private FontFamily segoeFontFamily;
        private Frame frame;

        public MainPage()
        {
            this.InitializeComponent();
            Window.Current.SetTitleBar(DragGrid);
            segoeFontFamily = new FontFamily("Segoe MDL2 Assets");
           
        }

        //透明背景与其深浅色转换
        private static Brush GetAppBackgroundBrush()
        {
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
                //Dark
                var color = Color.FromArgb(255, 36, 36, 36);
                var tintOpacity = 0.85f;

                return new HostBackdropAcrylicBrush()
                {
                    FallbackColor = color,
                    LuminosityColor = color,
                    TintOpacity = tintOpacity,
                    NoiseTextureUri = ToAppxUri("../Assets/noise_high.png"),
                };
            }
            else
            {
                var color = Color.FromArgb(255, 230, 230, 230);
                var tintOpacity = 0.85f;

                return new HostBackdropAcrylicBrush()
                {
                    FallbackColor = color,
                    LuminosityColor = color,
                    TintOpacity = tintOpacity,
                    NoiseTextureUri = ToAppxUri("../Assets/noise_high.png"),
                };
            }
        }

        private static Uri ToAppxUri(string path)
        {
            string prefix = $"ms-appx://{(path.StartsWith('/') ? string.Empty : "/")}";
            return new Uri($"{prefix}{path}");
        }

        private void navigationView_Loaded(object sender, RoutedEventArgs e)
        {
            var homeItem = new NavigationViewItem()
            {
                Content = "主页",
                Icon = new FontIcon()
                {
                    FontFamily = segoeFontFamily,
                    Glyph = "\uE80F",
                },
                Tag = "Home",
            };

            var locationItem = new NavigationViewItem()
            {
                Content = "城市",
                Icon = new FontIcon()
                {
                    FontFamily = segoeFontFamily,
                    Glyph = "\uE707",
                },
                Tag = "Location",
            };

            navigationView.MenuItems.Add(homeItem);
            navigationView.MenuItems.Add(locationItem);
            
            navigationView.SelectedItem = homeItem;

            frame = new Frame();
            //frame.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            frame.Content = new HomePage();

            navigationView.Content = frame;

            navigationView.SelectionChanged += NavigationView_SelectionChanged;
        }

        private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                if (args.SelectedItem is NavigationViewItem item)
                {

                    switch (item.Tag)
                    {
                        case "Home":                         
                            frame.Content = new HomePage();
                            break;
                        case "Location":
                            frame.Content = new LocationPage();
                            break;
                        case "Settings":
                            frame.Content = new SettingsPage();
                            break;
                        
                        default:                           
                            frame.Content = new SettingsPage();
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

     
    }
}
