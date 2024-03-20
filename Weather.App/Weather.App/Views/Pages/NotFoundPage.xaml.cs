using Microsoft.UI.Xaml.Controls;

namespace Weather.App.Views.Pages;

public sealed partial class NotFoundPage : Page
{
    private static object _instance = new();

    public static NotFoundPage Instance
    {
        get
        {
            if (_instance is not NotFoundPage)
                _instance = new NotFoundPage();

            return _instance as NotFoundPage;
        }
    }

    public NotFoundPage()
    {
        InitializeComponent();
    }
}
