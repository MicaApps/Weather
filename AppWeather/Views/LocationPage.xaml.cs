using AppWeather.Models;
using AppWeather.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    public sealed partial class LocationPage : Page
    {
        public LocationPageViewModel ViewModel => (LocationPageViewModel)DataContext;

        public LocationPage()
        {
            this.InitializeComponent();

            DataContext = Ioc.Default.GetRequiredService<LocationPageViewModel>();

        }

    }
}
