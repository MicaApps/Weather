using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using Weather.App.Services;
using Weather.Core.Standards;

namespace Weather.App.ViewModels.Pages;

public class SelectApiPageViewModel : ObservableObject
{
    public SelectApiPageViewModel()
    {
        var adapters = PluginsService.Instance.RequestPlugins<IAdapter>();

        var names = adapters.Select(
            x => x.GetAdapterIdentity()
        ).Distinct().ToList();

        foreach (var name in names)
            Apis.Add(name);
    }

    private readonly ObservableCollection<string> apis = [];

    public ObservableCollection<string> Apis => apis;

    private string key;

    public string Key
    {
        get => key;
        set => SetProperty(ref key, value);
    }

    private int selectedIndex;

    public int SelectedIndex
    {
        get => selectedIndex;
        set => SetProperty(ref selectedIndex, value);
    }
}
