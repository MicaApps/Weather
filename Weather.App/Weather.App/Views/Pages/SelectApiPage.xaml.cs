using Microsoft.UI.Xaml.Controls;
using Weather.App.Configuration;
using Weather.App.ViewModels.Pages;

namespace Weather.App.Views.Pages;

public sealed partial class SelectApiPage : Page
{
    public SelectApiPage()
    {
        InitializeComponent();
    }

    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        AppConfig.Instance.Api.Key = (sender as TextBox).Text;
    }

    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var vm = DataContext as SelectApiPageViewModel;

        AppConfig.Instance.Api.ProviderIdentity = vm.Apis[(sender as ComboBox).SelectedIndex];
    }
}
