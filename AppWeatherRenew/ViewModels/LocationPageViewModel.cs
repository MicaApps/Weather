using CommunityToolkit.Mvvm.ComponentModel;

namespace AppCoreOld2.ViewModels;

internal partial class LocationPageViewModel // Properties
{
    // 请使用由 CommunityToolkit.Mvvm 生成的绑定属性，而不是直接引用该分部类下定义的字段

    [ObservableProperty]
    private bool _isBusy;
}

internal partial class LocationPageViewModel : ObservableObject
{
    LocationPageViewModel()
    {
        IsBusy = false;
    }
}
