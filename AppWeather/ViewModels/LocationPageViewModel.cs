using AppWeather.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;

namespace AppWeather.ViewModels
{
    public class LocationPageViewModel : ObservableObject
    {
        BackgroundWorker bgWorker = null;

        public ObservableCollection<SimpleWeatherClass> cityWeathers = new ObservableCollection<SimpleWeatherClass>();

        private ObservableCollection<SimpleWeatherClass> _cityWeathers;
        public ObservableCollection<SimpleWeatherClass> CityWeathers
        {
            get => _cityWeathers;
            set => SetProperty(ref _cityWeathers, value);
        }

        private SimpleWeatherClass _selectedCity;
        public SimpleWeatherClass SelectedCity
        {
            get => _selectedCity;
            set => SetProperty(ref _selectedCity, value);
        }

        private Visibility _progressRingVisbility;
        public Visibility ProgressRingVisbility
        {
            get => _progressRingVisbility;
            set => SetProperty(ref _progressRingVisbility, value);
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand SetAsMainCommand { get; }
        public IAsyncRelayCommand ClearCommand { get; }
        public ICommand DeleteCommand { get; }
        public IAsyncRelayCommand AddCommand { get; }


        public LocationPageViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);
            SetAsMainCommand = new RelayCommand(SetAsMainCity);
            ClearCommand = new AsyncRelayCommand(ClearCities);
            DeleteCommand = new RelayCommand(DeleteCity);
            AddCommand = new AsyncRelayCommand<Page>(AddCity);
            
        }

        private void Refresh()
        {
            ProgressRingVisbility = Visibility.Visible;
            IsBusy = true;

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += LoadCityBW_DoWork;
            bgWorker.RunWorkerCompleted += AddCityBW_RunWorkerCompleted;//当完成时回调
            bgWorker.RunWorkerAsync();
        }

        private void SetAsMainCity()
        {
            if (SelectedCity != null)
            {
                var weatherinfo = SelectedCity as SimpleWeatherClass;

                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;

                // load a setting that is local to the device
                localSettings.Values["Location"] = weatherinfo.CityName;
            }
        }

        private void DeleteCity()
        {
            if (SelectedCity != null)
            {
                var weatherinfo = SelectedCity as SimpleWeatherClass;
                string cityName = weatherinfo.CityName;
                int index = cityWeathers.ToList().FindIndex(x => x.CityName.Equals(cityName));
                if (index != -1)
                {
                    ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    // load a setting that is local to the device
                    String cities = localSettings.Values["Cities"] as string;
                    List<string> cityArray = cities.Split('&').ToList();
                    cityArray.Remove(cityName);
                    localSettings.Values["Cities"] = string.Join('&', cityArray);
                    cityWeathers.Remove(weatherinfo);
                }
            }
        }

        private async Task ClearCities()
        {
            ContentDialog cd = new ContentDialog() { Title = "AskClear", Content = "AllClear" };
            cd.PrimaryButtonText = "OK";
            cd.PrimaryButtonClick += (ss, ee) =>
            {
                cityWeathers.Clear();
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                localSettings.Values["Cities"] = string.Empty;
            };
            cd.CloseButtonText = "Cancle";
            await cd.ShowAsync();
        }

        private async Task AddCity(Page _page)
        {
            string striparr = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json");

            List<CityInfo> cities = JsonHelper.DeserializeJsonToList<CityInfo>(striparr);
            List<string> cityStrList = cities.Select(t => t.name).ToList();

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            TextBox textBox = new TextBox() { Width = 200, Height = 32, VerticalAlignment = VerticalAlignment.Center };
            ComboBox cityComboBox = new ComboBox() { Height = 32, VerticalAlignment = VerticalAlignment.Center };
            textBox.TextChanged += (s1, e1) =>
            {
                try
                {
                    var seletcity = cities.Where(p => p.name.ToLower().Contains(textBox.Text.ToLower())).First();
                    cityComboBox.SelectedIndex = cities.IndexOf(seletcity);
                }
                catch { }
            };
            cityComboBox.ItemsSource = cityStrList;
            panel.Children.Add(textBox);
            panel.Children.Add(cityComboBox);

            cityComboBox.SelectedIndex = 0;
            ContentDialog cd = new ContentDialog()
            {
                Content = panel,
                Title = "Add City",
            };
            cd.PrimaryButtonText = "OK";
            cd.PrimaryButtonClick += (s1, e1) =>
            {
                if (cityComboBox.SelectedIndex > -1)
                {
                    //CityView.Visibility = Visibility.Collapsed;
                    bgWorker = new BackgroundWorker();
                    bgWorker.DoWork += AddCityBW_DoWork;
                    bgWorker.RunWorkerCompleted += AddCityBW_RunWorkerCompleted;//当完成时回调
                    ProgressRingVisbility = Visibility.Visible;
                    IsBusy = true;
                    bgWorker.RunWorkerAsync(cities[cityComboBox.SelectedIndex].name);
                }
            };
            cd.CloseButtonText = "Cancle";

            cd.XamlRoot = _page.XamlRoot;
            await cd.ShowAsync();

        }

        private void LoadCityBW_DoWork(object sender, DoWorkEventArgs e)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            String cities = localSettings.Values["Cities"] as string;
            if (cities != null)
            {
                string[] cityArray = cities.Split('&');

                foreach (var city in cityArray)
                {
                    if (!string.IsNullOrEmpty(city))
                    {
                        SimpleWeatherClass weather = OpenWeather.GetSimpleWeather(city);
                        AddCityData(weather);
                    }

                }
            }

        }


        public int AddCityData(SimpleWeatherClass weather)
        {
            int index = 0;
            Task.Run(async () =>
            {
                //this part is run on a background thread...
                //如果没有该城市
                if (weather != null)
                {
                    if (weather.CityName != null)
                    {
                        index = cityWeathers.ToList().FindIndex(x => x.CityName.Equals(weather.CityName));
                        if (index == -1)
                        {
                            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                            //        () =>
                            //        {
                                        //this part is being executed back on the UI thread...
                                        cityWeathers.Add(weather);
                                    //});

                        }
                        else
                        {
                            //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
                            //        () =>
                            //        {
                                        //this part is being executed back on the UI thread...
                                        cityWeathers[index] = weather;
                                    //});
                            ////更新现有信息

                        }
                    }
                }

            });

            return index;
        }

        private void AddCityBW_DoWork(object sender, DoWorkEventArgs e)
        {
            string cityName = e.Argument.ToString();

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string cities = localSettings.Values["Cities"] as string;
            if (cities == null)
            {
                cities = string.Empty;
            }

            List<string> cityList = cities.Split('&').ToList();
            cityList.Add(cityName);
            localSettings.Values["Cities"] = string.Join('&', cityList);

            SimpleWeatherClass weather = OpenWeather.GetSimpleWeather(cityName);
            AddCityData(weather);
        }

        private void AddCityBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //CityView.Visibility = Visibility.Visible;
            ProgressRingVisbility = Visibility.Collapsed;
            IsBusy = false;
            CityWeathers = cityWeathers;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            cityWeathers.Clear();
            CityWeathers.Clear();
            CityWeathers = null;
        }

    }
}
