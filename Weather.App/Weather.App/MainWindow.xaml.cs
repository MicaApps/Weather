using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Weather.App.Configuration;
using Weather.App.Views.Pages;

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
        _ => typeof(NotFoundPage),
    };
}
