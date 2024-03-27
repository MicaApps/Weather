using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.App.Configuration;
using Weather.App.Services;
using Weather.Core.Models;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;

namespace Weather.App.ViewModels.Pages;

public class HomePageViewModel : ObservableObject
{
    public HomePageViewModel()
    {
        RefreshCommand = new RelayCommand(() => Refresh());
    }

    public HomePageViewModel Refresh()
    {
        RefreshCityInfo(refreshWeather: true);

        return this;
    }

    public HomePageViewModel RefreshCityInfo(bool refreshWeather = true)
    {
        var location = AppConfig.Instance.SelectedLocation;

        if (CacheService.Instance.TryQueryCity(location, out var city))
        {
            CityInfo = city;

            if (refreshWeather) RefreshWeatherInfo();
        }
        else
        {
            var queryers = PluginsService.Instance.RequestPlugins<ICityQueryer>()
                .Where(x => x.GetAdapterIdentity().Equals(AppConfig.Instance.Api.ProviderIdentity)
            );

            if (queryers.Any() == false) return this;

            Task.Run(async () =>
            {
                var queryer = queryers.First();

                var apiConfig = IApiConfigProvider.Default;

                apiConfig.Key = AppConfig.Instance.Api.Key;

                var cities = await queryer.FuzzyQuery(location, apiConfig);

                if (cities is null || cities.Any() == false) return;

                CacheService.Instance.AddCity(location, cities.First());

                RefreshCityInfo(refreshWeather: refreshWeather);
            });
        }

        return this;
    }

    public HomePageViewModel RefreshWeatherInfo()
    {
        if (CacheService.Instance.TryQueryWeather(CityInfo.Id, out var info))
        {
            WeatherInfo = info;
        }
        else
        {
            var queryers = PluginsService.Instance.RequestPlugins<IWeatherQueryer>()
                .Where(x => x.GetAdapterIdentity().Equals(AppConfig.Instance.Api.ProviderIdentity)
            );

            if (queryers.Any() == false) return this;

            Task.Run(async () =>
            {
                var queryer = queryers.First();

                var apiConfig = IApiConfigProvider.Default;

                apiConfig.Key = AppConfig.Instance.Api.Key;

                var weather = await queryer.QueryCurrentWeather(CityInfo.Id, apiConfig);

                if (weather is not null)
                {
                    CacheService.Instance.AddWeather(CityInfo.Id, weather);

                    RefreshWeatherInfo();
                }
            });
        }

        return this;
    }

    private CityInfo cityInfo = new();

    public CityInfo CityInfo
    {
        get => cityInfo;
        set
        {
            SetProperty(ref cityInfo, value);
            OnPropertyChanged(nameof(CityInfo));
        }
    }

    private WeatherInfo weatherInfo = new();

    public WeatherInfo WeatherInfo
    {
        get => weatherInfo;
        set
        {
            SetProperty(ref weatherInfo, value);
            OnPropertyChanged(nameof(WeatherInfo));
        }
    }

    public ICommand RefreshCommand { get; set; }
}
