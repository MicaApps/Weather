using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Weather.App.Configuration;
using Weather.App.Services;
using Weather.App.ViewModels.Pages;

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

        Check();
    }

    public void Check()
    {
        DispatcherQueue.TryEnqueue(async () =>
        {
            while (string.IsNullOrWhiteSpace(AppConfig.Instance.Api.Key) || string.IsNullOrWhiteSpace(AppConfig.Instance.Api.ProviderIdentity))
            {
                var dialog = new ContentDialog
                {
                    XamlRoot = XamlRoot,
                    Style = Application.Current.Resources["DefaultContentDialogStyle"] as Style,
                    CloseButtonText = "OK",
                    Content = new SelectApiPage()
                };

                _ = await dialog.ShowAsync();
            }

            while (string.IsNullOrWhiteSpace(AppConfig.Instance.SelectedLocation))
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

            (DataContext as HomePageViewModel).Refresh(updateCity: false, updateWeather: false);
        });
    }
}
