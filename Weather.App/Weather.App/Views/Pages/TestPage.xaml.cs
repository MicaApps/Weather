using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Weather.App.Services;
using Weather.App.ViewModels.Pages;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Weather.App.Views.Pages;

public sealed partial class TestPage : Page
{
    private static object _instance = new();

    public static TestPage Instance
    {
        get
        {
            if (_instance is not TestPage)
                _instance = new TestPage();

            return _instance as TestPage;
        }
    }

    public TestPage()
    {
        InitializeComponent();

        (DataContext as TestPageViewModel).ShowDialog = async (content) =>
        {
            var dialog = new ContentDialog
            {
                XamlRoot = XamlRoot,
                Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                Title = "Requested Result",
                CloseButtonText = "OK",
                DefaultButton = ContentDialogButton.Close,
                Content = content
            };

            _ = await dialog.ShowAsync();
        };
    }
}
