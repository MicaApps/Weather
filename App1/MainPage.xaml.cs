using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.WindowManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace App1
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();



        }




        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            AppWindow appWindow = await AppWindow.TryCreateAsync();
            if (appWindow != null)
            {
                // 创建窗口内容
                Frame appWindowContentFrame = new Frame();


                appWindowContentFrame.Navigate(typeof(BlankPage1)); // 假设你有一个名为MainPage的页面

                // 将XAML内容附加到AppWindow
                ElementCompositionPreview.SetAppWindowContent(appWindow, appWindowContentFrame);

                // 显示新窗口
                await appWindow.TryShowAsync();
            }
        }
    }
}
