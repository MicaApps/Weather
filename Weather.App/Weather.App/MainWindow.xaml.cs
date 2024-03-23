using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Weather.App.Configuration;
using Weather.App.Services;
using Weather.App.Views;
using Weather.App.Views.Pages;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Weather.App;

public sealed partial class MainWindow : Window
{
    private static object _instance = new();

    public static MainWindow Instance
    {
        get
        {
            if (_instance is not MainWindow)
                _instance = new MainWindow();

            return _instance as MainWindow;
        }
    }

    private MainWindow()
    {
        ExtendsContentIntoTitleBar = true;

        InitializeComponent();

        Closed += (_, _) =>
        {
            AppConfig.Instance.Save();
        };
    }

    private void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        var selectedItem = args.SelectedItem as NavigationViewItem;

        MainFrame.Navigate(GetPageTypeFromName(selectedItem.Tag as string));
    }

    private static Type GetPageTypeFromName(string name) => name switch
    {
        "HomePage" => typeof(HomePage),
        "CityPage" => typeof(CityPage),
        "TestPage" => typeof(TestPage),
        _ => typeof(NotFoundPage),
    };
}
