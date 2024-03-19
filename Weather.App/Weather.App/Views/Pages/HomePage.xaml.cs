using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace Weather.App.Views.Pages;

public sealed partial class HomePage : Page
{
    private static object _instance = new();

    public static HomePage Instance
    {
        get
        {
            if (_instance is not HomePage)
                _instance = new HomePage();

            return _instance as HomePage;
        }
    }

    public HomePage()
    {
        InitializeComponent();
    }
}
