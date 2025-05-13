#nullable enable
using AppWeatherRe.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWeatherRe.ViewModels;

public class AddCityContentDialogViewModel : ObservableObject
{
    public CityInfo? SelectedCity { get; set; }

    public ObservableCollection<CityInfo> CityNames { get; set; } = [];

    public async Task SearchCitiesAsync(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
        {
            CityNames.Clear();
            return;
        }

        try
        {
            var response = await WeatherAdapter.GetLocationInformationAsync(cityName);
            if (response != null && response.location != null)
            {
                CityNames.Clear();
                foreach (var city in response.location)
                {
                    CityNames.Add(new CityInfo
                    {
                        CityName = city.displayName,
                        Location = city.country,
                        PlaceId = city.placeId,
                    });
                }
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}

public record CityInfo
{
    public string CityName { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    public override string ToString() => CityName + "-" + Location;

    public string PlaceId { get; set; } = string.Empty;
}