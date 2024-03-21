using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Weather.App.Services;
using Weather.Core.Standards.Query;
using Weather.Core.Standards.WebApi;

namespace Weather.App.ViewModels.Pages;

public class TestPageViewModel : ObservableObject
{
    public TestPageViewModel()
    {
        QueryCityCommand = new RelayCommand(QueryCity);
    }

    private string key;

    public string Key
    {
        get => key;
        set => SetProperty(ref key, value);
    }

    private string location;

    public string Location
    {
        get => location;
        set => SetProperty(ref location, value);
    }

    private string cityQueryResult;

    public string CityQueryResult
    {
        get => cityQueryResult;
        set => SetProperty(ref cityQueryResult, value);
    }

    public ICommand QueryCityCommand { get; }

    private async void QueryCity()
    {
        var queryers = PluginsService.Instance.RequestPlugins<ICityQueryer>();

        if (queryers.Any() == false)
            throw new InvalidOperationException();

        var queryer = queryers.First();

        var apiConfig = IApiConfigProvider.Default;

        apiConfig.Key = Key;

        var cities = await queryer.FuzzyQuery(Location, apiConfig);

        var text = JsonSerializer.Serialize(cities.ToList(), new JsonSerializerOptions()
        {
            WriteIndented = true,
            IncludeFields = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        });

        CityQueryResult = text;
    }
}
