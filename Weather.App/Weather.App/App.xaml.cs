using Microsoft.UI.Xaml;
using Weather.App.Configuration;
using Weather.App.Services;

namespace Weather.App;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();

        PluginsService.Instance.Initialize();

        CacheService.Instance.Initialize();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        AppConfig.Instance.RanTime++;

        AppConfig.Instance.Save();

        MainWindow.Instance.Activate();
    }
}
