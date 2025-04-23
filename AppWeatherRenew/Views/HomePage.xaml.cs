using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityPlayer;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace AppCoreOld2.Views;

/// <summary>
/// 可用于自身或导航至 Frame 内部的空白页。
/// </summary>
public sealed partial class HomePage : Page
{
    public HomePage()
    {
        this.InitializeComponent();

        AppCallbacks appCallbacks = AppCallbacks.Instance;

        // k1mlka: 300 MB 左右的内存占用，在未激活或者其他页面时存在消息循环挂钩 CPU 占用 0.几%，激活时为 Unity 侧渲染的占用
        appCallbacks.RenderingStarted += () => { };
        appCallbacks.SetSwapChainPanel(m_DXSwapChainPanel);
        appCallbacks.SetCoreWindowEvents(Window.Current.CoreWindow);
        appCallbacks.InitializeD3DXAML();
    }
}
