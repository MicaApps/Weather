using AppWeatherRe.Helpers;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage;
using Windows.UI.Xaml.Controls;

namespace AppWeatherRe.ViewModels;

internal partial class LocationPageViewModel // Properties
{
    // 请使用由 CommunityToolkit.Mvvm 生成的绑定属性，而不是直接引用该分部类下定义的字段

    /// <summary>
    /// 控制 ProgressRing 的显示
    /// </summary>
    [ObservableProperty]
    private bool _isBusy;
}

internal partial class LocationPageViewModel : ObservableObject
{
    /// <summary>
    /// 数据列表
    /// </summary>
    public ObservableCollection<SimpleWeatherClass> CityWeathers { get; set; } = new();

    public ICommand AddCommand { get; }

    public LocationPageViewModel()
    {
        AddCommand = new AsyncRelayCommand(AddCity);
    }

    private async Task AddCity()
    {
        var vm = new AddCityContentDialogViewModel();
        var dialog = new Views.AddCityContentDialog()
        {
            DataContext = vm,
        };
        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary && 
            vm.SelectedCity is CityInfo city &&
            !CityWeathers.Any(a => a.CityName == city.CityName))
        {
            // 基本上就是完成后控制 Progress 几个的显示隐藏状态
            //IsBusy = true;

            SavePlaceIdToSettings(city.PlaceId);

            var weather = await WeatherAdapter.GetSimpleWeatherAsync(city.PlaceId);
            CityWeathers.Add(weather);
        }
    }

    void SavePlaceIdToSettings(string placeId)
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        if (localSettings.Values["placeId"] is not string placeIds)
        {
            placeIds = string.Empty;
        }

        // 这里使用 & 分割多个 PlaceId
        List<string> placeIdList = [.. placeIds.Split('&')];
        placeIdList.Add(placeId);
        localSettings.Values["placeId"] = string.Join('&', placeIdList);
    }
}
