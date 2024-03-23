using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Weather.App.Configuration;
using Weather.App.Services;

namespace Weather.App.Views.Pages;

public sealed partial class HomePage : Page
{
    private static object _instance = new();

    public static HomePage Instance
    {
        get
        {
            if (_instance is not HomePage)
                _instance = new HomePage();

            return _instance as HomePage;
        }
    }

    public HomePage()
    {
        InitializeComponent();

        DispatcherQueue.TryEnqueue(async () =>
        {
            while (string.Empty.Equals(AppConfig.Instance.SelectedLocation))
            {
                var dialog = new ContentDialog
                {
                    XamlRoot = XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    //Background = new AcrylicBrush
                    //{
                    //    TintColor = Colors.Transparent,
                    //    FallbackColor = Colors.Transparent,
                    //    TintOpacity = 1,
                    //},
                    CloseButtonText = "OK",
                    Content = new SelectCityPage()
                };

                _ = await dialog.ShowAsync();
            }

            EventService.Invoke(nameof(EventService.SelectLocationOver), null);
        });
    }
}
