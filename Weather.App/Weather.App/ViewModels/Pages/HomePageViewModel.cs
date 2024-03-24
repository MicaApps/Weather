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

    public HomePageViewModel Refresh(bool updateCity = false, bool updateWeather = true)
    {
        var location = AppConfig.Instance.SelectedLocation;

        if (!updateCity && CacheService.Instance.TryQueryCity(location, out var city))
            CityInfo = city;
        else
        {
            var queryers = PluginsService.Instance.RequestPlugins<ICityQueryer>()
                .Where(x => x.GetAdapterIdentity().Equals(AppConfig.Instance.Api.ProviderIdentity)
            );

            if (queryers.Any())
                Task.Run(async () =>
                {
                    var queryer = queryers.First();

                    var apiConfig = IApiConfigProvider.Default;

                    apiConfig.Key = AppConfig.Instance.Api.Key;

                    var cities = await queryer.FuzzyQuery(location, apiConfig);

                    if (cities.Any())
                        CacheService.Instance.AddCity(location, cities.First());

                    Refresh(updateCity: false, updateWeather: false);
                });

            return this;
        }

        if (!updateWeather && CacheService.Instance.TryQueryWeather(CityInfo.Id, out var info))
            WeatherInfo = info;
        else
        {
            var queryers = PluginsService.Instance.RequestPlugins<IWeatherQueryer>()
                .Where(x => x.GetAdapterIdentity().Equals(AppConfig.Instance.Api.ProviderIdentity)
            );

            if (queryers.Any())
                Task.Run(async () =>
                {
                    var queryer = queryers.First();

                    var apiConfig = IApiConfigProvider.Default;

                    apiConfig.Key = AppConfig.Instance.Api.Key;

                    var weather = await queryer.QueryCurrentWeather(CityInfo.Id, apiConfig);

                    if (weather is not null)
                        CacheService.Instance.AddWeather(CityInfo.Id, weather);

                    Refresh(updateCity: false, updateWeather: false);
                });

            return this;
        }

        return this;
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

    public ICommand RefreshCommand { get; set; }
}
