using AppWeather.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using UnityPlayer;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Core;
using Windows.Graphics.Display;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace AppWeather
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    public sealed partial class App : Application
    {
        private AppCallbacks m_AppCallbacks;
        public SplashScreen splashScreen;
        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public new static App Current => (App)Application.Current;

        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 已执行，逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            SetupOrientation();
            m_AppCallbacks = new AppCallbacks();
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 将在启动应用程序以打开特定文件等情况下使用。
        /// </summary>
        /// <param name="e">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            splashScreen = e.SplashScreen;
            m_AppCallbacks.SetAppArguments(e.Arguments);
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                // Register services
                Ioc.Default.ConfigureServices(
                    new ServiceCollection()
                    .AddTransient<HomePageViewModel>()
                    .AddTransient<LocationPageViewModel>()
                    .BuildServiceProvider());

                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                
                if(localSettings.Values["Theme"] == null)
                {
                    localSettings.Values["Theme"] = "Default";
                    rootFrame.RequestedTheme = ElementTheme.Default;
                }
                else
                {
                    if (localSettings.Values["Theme"].Equals("Dark"))
                        rootFrame.RequestedTheme = ElementTheme.Dark;
                    else if (localSettings.Values["Theme"].Equals("Light"))
                        rootFrame.RequestedTheme = ElementTheme.Light;
                    else
                        rootFrame.RequestedTheme = ElementTheme.Default;
                }

                var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
                var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();
                //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                // load a setting that is local to the device
                String theme = localSettings.Values["Theme"] as string;
                if (theme != null)
                {
                    if (theme.Equals("Dark"))
                    {
                        uiTheme = "#FF000000";
                    }
                    else if (theme.Equals("Light"))
                    {
                        uiTheme = "#FFFFFFFF";
                    }
                }

                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                //拿到目前的窗口标题栏实例
                var view = ApplicationView.GetForCurrentView();

                //最大化最小化关闭按钮透明化
                view.TitleBar.BackgroundColor = Colors.Transparent;
                view.TitleBar.ButtonBackgroundColor = Colors.Transparent;

                if (uiTheme == "#FF000000")
                {
                    //Dark
                    view.TitleBar.ButtonForegroundColor = Colors.White;
                    //深色模式下状态栏按钮悬浮颜色改一下
                    view.TitleBar.ButtonHoverBackgroundColor = Colors.Gray;
                    //深色模式下状态栏按钮按下颜色改一下
                    view.TitleBar.ButtonPressedBackgroundColor = Colors.Gray;
                }
                else
                {
                    //Light
                    view.TitleBar.ButtonForegroundColor = Colors.Black;
                    //浅色模式下状态栏按钮悬浮颜色改一下
                    view.TitleBar.ButtonHoverBackgroundColor = Colors.LightGray;
                    //浅色模式下状态栏按钮按下颜色改一下
                    view.TitleBar.ButtonPressedBackgroundColor = Colors.LightGray;
                }

                view.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.InactiveBackgroundColor = Colors.Transparent;
                view.TitleBar.ForegroundColor = Colors.White;

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // 当导航堆栈尚未还原时，导航到第一页，
                    // 并通过将所需信息作为导航参数传入来配置
                    // 参数
                    //rootFrame.Navigate(typeof(Views.BlankPage1), e.Arguments);
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // 确保当前窗口处于活动状态
                Window.Current.Activate();
            }
        }
         

        void SetupOrientation()
        {
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Landscape | DisplayOrientations.LandscapeFlipped | DisplayOrientations.Portrait | DisplayOrientations.PortraitFlipped;
            // ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.FullScreen;
        }
        /// <summary>
        /// 导航到特定页失败时调用
        /// </summary>
        ///<param name="sender">导航失败的框架</param>
        ///<param name="e">有关导航失败的详细信息</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。  在不知道应用程序
        /// 无需知道应用程序会被终止还是会恢复，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
