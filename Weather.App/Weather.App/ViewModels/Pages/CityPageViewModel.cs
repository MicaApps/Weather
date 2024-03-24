using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Weather.Core.Models;

namespace Weather.App.ViewModels.Pages;

public class CityPageViewModel : ObservableObject
{
    public CityPageViewModel()
    {

    }

    private readonly ObservableCollection<CityInfo> cityInfos = [];

    public ObservableCollection<CityInfo> CityInfos => cityInfos;
}
