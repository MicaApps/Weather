using AppCoreOld2.Views;
using UnityPlayer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace AppCoreOld2
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            ContentFrame.Navigate(typeof(HomePage));
        }

        private void NavigationView_SelectionChanged(object sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is not Microsoft.UI.Xaml.Controls.NavigationViewItem item)
                return;

            switch (item.Tag.ToString())
            {
                case "Home":
                    ContentFrame.Navigate(typeof(HomePage));
                    break;
                case "Location":
                    //ContentFrame.Navigate(typeof(LocationPage));
                    break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ;
        }
    }
}
