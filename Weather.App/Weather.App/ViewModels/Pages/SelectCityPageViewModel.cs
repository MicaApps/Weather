using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.App.Configuration;
using Weather.App.Services;
using Weather.Core.Models;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;

namespace Weather.App.ViewModels.Pages;

public class SelectCityPageViewModel : ObservableObject
{
    public SelectCityPageViewModel()
    {
        QueryCityCommand = new RelayCommand(QueryCity);
    }

    private string location;

    public string Location
    {
        get => location;
        set => SetProperty(ref location, value);
    }

    private int selectedIndex;

    public int SelectedIndex
    {
        get => selectedIndex;
        set => SetProperty(ref selectedIndex, value);
    }

    private readonly ObservableCollection<CityInfo> cityInfos = [];

    public ObservableCollection<CityInfo> CityInfos => cityInfos;

    public ICommand QueryCityCommand { get; }

    private async void QueryCity()
    {
        CityInfos.Clear();

        var queryers = PluginsService.Instance.RequestPlugins<ICityQueryer>();

        if (queryers.Any() == false)
            throw new InvalidOperationException();

        var queryer = queryers.First();

        var apiConfig = IApiConfigProvider.Default;

        apiConfig.Key = AppConfig.Instance.Api.Key;

        var cities = await queryer.FuzzyQuery(Location, apiConfig);

        if (cities.Any() == false)
            CityInfos.Add(new CityInfo
            {
                Name = "No result found."
            });
        else
            foreach (var city in cities.Take(5))
                CityInfos.Add(city);

        SelectedIndex = CityInfos.Count > 0 ? 0 : -1;
    }
}
