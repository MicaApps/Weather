using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

public class DailyWeatherForecastPageViewModel : ObservableObject
{
    private static CityInfo GetCurrentCity()
    {
        var location = AppConfig.Instance.SelectedLocation;

        if (CacheService.Instance.TryQueryCity(location, out var city))
            return city;

        return null;
    }

    public DailyWeatherForecastPageViewModel()
    {
        RefreshCommand = new RelayCommand(() => Refresh(GetCurrentCity()));
    }

    public ICommand RefreshCommand { get; set; }

    public DailyWeatherForecastPageViewModel Refresh(CityInfo city)
    {
        if (city is null) return this;

        if (CacheService.Instance.TryQueryDailyWeatherForecast(city.Id, out var infos))
        {
            DailyWeatherInfos.Clear();

            foreach (var item in infos)
                DailyWeatherInfos.Add(item);

            return this;
        }
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

                    var weathers = await queryer.QueryDailyWeatherForecast(city.Id, apiConfig);

                    if (weathers is null) return;

                    if (weathers.Any())
                        CacheService.Instance.AddDailyWeatherForecast(city.Id, weathers);

                    Refresh(city);
                });

            return this;
        }
    }

    private readonly ObservableCollection<WeatherInfo> dailyWeatherInfos = [];

    public ObservableCollection<WeatherInfo> DailyWeatherInfos => dailyWeatherInfos;
}
