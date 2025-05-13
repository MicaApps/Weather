using AppWeatherRe.ViewModels;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace AppWeatherRe.Views;

public sealed partial class AddCityContentDialog : ContentDialog
{
    public AddCityContentDialog()
    {
        this.InitializeComponent();
    }

    private void CitySearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        if (args.SelectedItem is not CityInfo cityInfo)
            return;

        // 选择后只显示城市不显示 Location
        sender.Text = cityInfo.CityName;
        var viewModel = (AddCityContentDialogViewModel)DataContext;
        viewModel.SelectedCity = cityInfo;
    }

    private CancellationTokenSource _delayTokenSource;

    private async void CitySearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        // 确保文本是由用户输入更改的
        if (args.Reason != AutoSuggestionBoxTextChangeReason.UserInput)
            return;

        var viewModel = (AddCityContentDialogViewModel)DataContext;

        // 取消之前的延迟操作（如果有）
        _delayTokenSource?.Cancel();
        _delayTokenSource = new CancellationTokenSource();

        var token = _delayTokenSource.Token;
        string searchText = sender.Text;

        try
        {
            // 延迟500毫秒
            await Task.Delay(500, token);

            // 如果延迟没有被取消，执行搜索操作
            if (!token.IsCancellationRequested)
            {
                await viewModel.SearchCitiesAsync(searchText);
            }
        }
        catch (TaskCanceledException)
        {
            // 延迟被取消，不执行任何操作
        }
    }
}