using Microsoft.UI.Xaml.Controls;
using Weather.App.Services;
using Weather.App.ViewModels.Pages;

namespace Weather.App.Views.Pages;

public sealed partial class SelectCityPage : Page
{
    public SelectCityPage()
    {
        InitializeComponent();
    }

    private void SearchTextBox_KeyDown(object sender, Microsoft.UI.Xaml.Input.KeyRoutedEventArgs e)
    {
        if (e.Key == Windows.System.VirtualKey.Enter)
        {
            var vm = (DataContext as SelectCityPageViewModel);

            vm.Location = (sender as TextBox).Text;

            vm.QueryCityCommand.Execute(null);
        }
    }

    private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var index = (sender as ListView).SelectedIndex;

        if (index >= 0)
        {
            var vm = (DataContext as SelectCityPageViewModel);

            var cityId = vm.CityInfos[index].Id;

            EventService.Invoke(nameof(EventService.SelectedLocationChanged), [cityId]);
        }
    }
}
