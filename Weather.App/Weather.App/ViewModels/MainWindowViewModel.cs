using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using Weather.App.Configuration;
using Weather.App.Services;

namespace Weather.App.ViewModels;

public class MainWindowViewModel : ObservableObject
{
    public MainWindowViewModel()
    {
        EventService.SelectLocationOver += () =>
        {
            if (string.IsNullOrWhiteSpace(AppConfig.Instance.SelectedLocation) == false)
                MaskVisibility = Visibility.Collapsed;
        };
    }

    private Visibility maskVisibility = string.IsNullOrWhiteSpace(AppConfig.Instance.SelectedLocation) ? Visibility.Visible : Visibility.Collapsed;

    public Visibility MaskVisibility
    {
        get => maskVisibility;
        set => SetProperty(ref maskVisibility, value);
    }
}
