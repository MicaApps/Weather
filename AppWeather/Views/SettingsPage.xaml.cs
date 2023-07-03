using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AppWeather.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();
        }

        private void SettingsPageLoaded(object sender, RoutedEventArgs e)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            String theme = localSettings.Values["Theme"] as string;

            if (theme.Equals("Dark"))
            {
                DarkThemeRdoBtn.IsChecked = true;
            }
            else if (theme.Equals("Light"))
            {
                LightThemeRdoBtn.IsChecked = true;
            }
            else
            {
                DefaultThemeRdoBtn.IsChecked = true;
            }

            CelsiusRdoBtn.IsChecked = true;
        }

        private void OnThemeRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var selectedTheme = ((RadioButton)sender)?.Tag?.ToString();

            var currentView = ApplicationView.GetForCurrentView();

            if (selectedTheme != null)
            {
                if (selectedTheme.Equals("Dark"))
                {
                    ApplicationData.Current.LocalSettings.Values["Theme"] = "Dark";
                    (Window.Current.Content as Frame).RequestedTheme = ElementTheme.Dark;
                    currentView.TitleBar.ButtonForegroundColor = Colors.White;
                    //深色模式下状态栏按钮悬浮颜色改一下
                    currentView.TitleBar.ButtonHoverBackgroundColor = Colors.Gray;
                    //深色模式下状态栏按钮按下颜色改一下
                    currentView.TitleBar.ButtonPressedBackgroundColor = Colors.Gray;
                }
                else if (selectedTheme.Equals("Light"))
                {
                    ApplicationData.Current.LocalSettings.Values["Theme"] = "Light";
                    (Window.Current.Content as Frame).RequestedTheme = ElementTheme.Light;
                    currentView.TitleBar.ButtonForegroundColor = Colors.Black;
                    //浅色模式下状态栏按钮悬浮颜色改一下
                    currentView.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                    //浅色模式下状态栏按钮按下颜色改一下
                    currentView.TitleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                }
                else
                {
                    ApplicationData.Current.LocalSettings.Values["Theme"] = "Default";
                    (Window.Current.Content as Frame).RequestedTheme = ElementTheme.Default;
                    var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
                    var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();
                    if (uiTheme == "#FF000000")
                    {
                        //Dark
                        currentView.TitleBar.ButtonForegroundColor = Colors.White;
                        //深色模式下状态栏按钮悬浮颜色改一下
                        currentView.TitleBar.ButtonHoverBackgroundColor = Colors.Gray;
                        //深色模式下状态栏按钮按下颜色改一下
                        currentView.TitleBar.ButtonPressedBackgroundColor = Colors.Gray;
                    }
                    else
                    {
                        //Light
                        currentView.TitleBar.ButtonForegroundColor = Colors.Black;
                        //浅色模式下状态栏按钮悬浮颜色改一下
                        currentView.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                        //浅色模式下状态栏按钮按下颜色改一下
                        currentView.TitleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                    }
                }
            }

        }

        private void OnUnitRadioButtonChecked(object sender, RoutedEventArgs e)
        {
            var unit = ((RadioButton)sender)?.Tag?.ToString();

            if (unit != null)
            {
                if (unit.Equals("Celsius"))
                {
                    ApplicationData.Current.LocalSettings.Values["TemperatureUnit"] = "Celsius";                   
                }
                else if (unit.Equals("Fahrenheit"))
                {
                    ApplicationData.Current.LocalSettings.Values["TemperatureUnit"] = "Fahrenheit";               
                }
                else
                {
                    //Kelvin
                    ApplicationData.Current.LocalSettings.Values["TemperatureUnit"] = "Kelvin";
                }
            }
        }

    }
}
