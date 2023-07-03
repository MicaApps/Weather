using AppWeather.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace AppWeather.ViewModels
{
    public class HomePageViewModel : ObservableObject
    {
        public string cityName = "Shanghai";
        public List<WeatherClass> weathers;
        public BackgroundWorker bgWorker = null;
        public ObservableCollection<EveryDayWeather> everyDayWeathers = new ObservableCollection<EveryDayWeather>();

        //Light or Dark
        private string _theme;
        public string Theme
        {
            get => _theme;
            set => SetProperty(ref _theme, value);
        }

        private SolidColorBrush _themeForeground;
        public SolidColorBrush ThemeForeground
        {
            get => _themeForeground;
            set => SetProperty(ref _themeForeground, value);
        }

        private SolidColorBrush _chartBackground;
        public SolidColorBrush ChartBackground
        {
            get => _chartBackground;
            set => SetProperty(ref _chartBackground, value);
        }

        private string _ellipse1;
        public string Ellipse1
        {
            get => _ellipse1;
            set => SetProperty(ref _ellipse1, value);
        }

        private string _ellipse2;
        public string Ellipse2
        {
            get => _ellipse2;
            set => SetProperty(ref _ellipse2, value);
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

        private string _locationName;
        public string LocationName
        {
            get => _locationName;
            set => SetProperty(ref _locationName, value);
        }

        private string _weather;
        public string Weather
        {
            get => _weather;
            set => SetProperty(ref _weather, value);
        }

        private string _tip;

        public string Tip
        {
            get => _tip;
            set => SetProperty(ref _tip, value);
        }

        private string _currentTemp;

        public string CurrentTemp
        {
            get => _currentTemp;
            set => SetProperty(ref _currentTemp, value);
        }

        private string _feelsLike;

        public string FeelsLike
        {
            get => _feelsLike;
            set => SetProperty(ref _feelsLike, value);
        }

        private string _humidity;

        public string Humidity
        {
            get => _humidity;
            set => SetProperty(ref _humidity, value);
        }

        private string _pressure;

        public string Pressure
        {
            get => _pressure;
            set => SetProperty(ref _pressure, value);
        }

        private string _vsibility;

        public string Vsibility
        {
            get => _vsibility;
            set => SetProperty(ref _vsibility, value);
        }

        private string _windSpeed;

        public string WindSpeed
        {
            get => _windSpeed;
            set => SetProperty(ref _windSpeed, value);
        }

        private double _windDegree;

        public double WindDegree
        {
            get => _windDegree;
            set => SetProperty(ref _windDegree, value);
        }

        private string _rainfall;

        public string Rainfall
        {
            get => _rainfall;
            set => SetProperty(ref _rainfall, value);
        }

        private string _sunrise;

        public string Sunrise
        {
            get => _sunrise;
            set => SetProperty(ref _sunrise, value);
        }

        private string _sunset;

        public string Sunset
        {
            get => _sunset;
            set => SetProperty(ref _sunset, value);
        }

        private string _currentWeatherImg;

        public string CurrentWeatherImg
        {
            get => _currentWeatherImg;
            set => SetProperty(ref _currentWeatherImg, value);
        }

        private string _todayTemperature;

        public string TodayTemperature
        {
            get => _todayTemperature;
            set => SetProperty(ref _todayTemperature, value);
        }

        private ObservableCollection<EveryDayWeather> _everyDayWeathers;

        public ObservableCollection<EveryDayWeather> EveryDayWeathers
        {
            get => _everyDayWeathers;
            set => SetProperty(ref _everyDayWeathers, value);
        }

        private ObservableCollection<HourWeather> _hourWeatherList;

        public ObservableCollection<HourWeather> HourWeatherList
        {
            get => _hourWeatherList;
            set => SetProperty(ref _hourWeatherList, value);
        }

        public ICommand RefreshCommand { get; }

        public HomePageViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);

            var DefaultTheme = new Windows.UI.ViewManagement.UISettings();
            var uiTheme = DefaultTheme.GetColorValue(Windows.UI.ViewManagement.UIColorType.Background).ToString();

            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
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

            if (uiTheme.Equals("#FF000000"))
            {
                //dark
                //分割线
                SolidColorBrush solidColorBrush = new SolidColorBrush();
                solidColorBrush.Opacity = 0.2;
                solidColorBrush.Color = Color.FromArgb(255, 255, 255, 255);
                ThemeForeground = solidColorBrush;

                Theme = "#FFFFFFFF";
                Ellipse1 = "ms-appx:///Assets/Ellipse 15.png";
                Ellipse2 = "ms-appx:///Assets/Ellipse 16.png";
            }
            else
            {
                //light
                SolidColorBrush solidColorBrush = new SolidColorBrush();
                solidColorBrush.Opacity = 0.2;
                solidColorBrush.Color = Color.FromArgb(255, 0, 0, 0);
                ThemeForeground = solidColorBrush;

                Theme = "#CC000000";
                Ellipse1 = "ms-appx:///Views/Images/Ellipse 15_Dark.png";
                Ellipse2 = "ms-appx:///Views/Images/Ellipse 16_Dark.png";
            }

        }

        private void Refresh()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            String homeCity = localSettings.Values["Location"] as string;

            if (!String.IsNullOrEmpty(homeCity))
                cityName = homeCity;
            else
                //set default value
                cityName = "Shanghai";

            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += refreshWeatherBW_DoWork;//工作线程回调，将要执行的代码放在此函数里    
            bgWorker.RunWorkerCompleted += refreshWeatherBW_RunWorkerCompleted;//当完成时回调
            ProgressRingVisbility = Visibility.Visible;
            IsBusy = true;
            bgWorker.RunWorkerAsync();
        }

        private void refreshWeatherBW_DoWork(object sender, DoWorkEventArgs e)
        {
            weathers = OpenWeather.GetWeathers(cityName);
        }

        private void refreshWeatherBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (weathers != null)
            {
                long time = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                string currentTime = DateTime.Now.ToString("u");
                string currentDate = currentTime.Split(' ')[0];
                if (weathers.Count > 0)
                {
                    //获取最接近当前时间的天气信息
                    WeatherClass currentWeather = weathers.OrderBy(x => Math.Abs((long)x.TimeStamp - time)).First(); //weathers[index];
                    LocationName = cityName;
                    Weather = currentWeather.State;
                    Tip = "";
                    if (Weather.Equals("Clouds"))
                    {
                        CurrentWeatherImg = "../Images/Cloud.png";
                    }
                    else if (Weather.Equals("Rain"))
                    {
                        CurrentWeatherImg = "../Images/Rainny.png";
                    }
                    else if (Weather.Equals("Clear"))
                    {
                        CurrentWeatherImg = "../Images/Clear.png";
                    }
                    //当前温度
                    double temp = double.Parse(currentWeather.Temp);
                    CurrentTemp = temp.ToString("0.0") + "℃";

                    FeelsLike = currentWeather.FeelsLike.ToString("0.0") + "℃";
                    //今日最低最高温度
                    double todayMinTemp = weathers.Where(i => i.Time.Split(" ")[0].Equals(currentDate)).Min(k => k.MinTemp);
                    double todayMaxTemp = weathers.Where(i => i.Time.Split(" ")[0].Equals(currentDate)).Max(k => k.MaxTemp);
                    TodayTemperature = todayMinTemp.ToString("0.0") + "℃-" + todayMaxTemp.ToString("0.0") + "℃";

                    Humidity = currentWeather.Humidity.ToString();
                    Pressure = currentWeather.Pressure.ToString();
                    Vsibility = currentWeather.Vsibility.ToString("0.0") + "km";
                    WindSpeed = currentWeather.WindSpeed.ToString();
                    WindDegree = currentWeather.WindDegree;
                    Rainfall = currentWeather.Rainfall.ToString("0.0") + "%";
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//当地时区
                    Sunrise = startTime.AddSeconds(currentWeather.Sunrise).ToLongTimeString().ToString();
                    Sunset = startTime.AddSeconds(currentWeather.Sunset).ToLongTimeString().ToString();

                    everyDayWeathers.Clear();
                    for (int i = 0; i < 6; i++)
                    {
                        string ttime = DateTime.Now.AddDays(i).ToString("u");
                        string tDate = ttime.Split(' ')[0];
                        string weather = weathers.Where(k => k.Time.Split(" ")[0].Equals(tDate)).First().State;
                        string img = "\uE9BD";
                        if (weather.Equals("Clouds"))
                        {
                            img = "\uE9BF";
                        }
                        else if (weather.Equals("Rain"))
                        {
                            img = "\uE9E2";
                        }
                        else if (weather.Equals("Sunny"))
                        {
                            img = "\uE9BD";
                        }

                        double tMinTemp = weathers.Where(k => k.Time.Split(" ")[0].Equals(tDate)).Min(k => k.MinTemp);
                        double tMaxTemp = weathers.Where(j => j.Time.Split(" ")[0].Equals(tDate)).Max(k => k.MaxTemp);
                        everyDayWeathers.Add(new EveryDayWeather { Img = img, Weather = weather, MinTemp = tMinTemp.ToString("0.0"), MaxTemp = tMaxTemp.ToString("0.0") });
                    }

                    EveryDayWeathers = new ObservableCollection<EveryDayWeather>(everyDayWeathers.Distinct().ToList());

                    GetChartData();

                }

            }

            ProgressRingVisbility = Visibility.Collapsed;
            IsBusy = false;
        }

        private void GetChartData()
        {
            if (weathers != null)
            {
                List<HourWeather> weatherList = new List<HourWeather>();
                for (int i = 0; i < 12; i++)
                {
                    weatherList.Add(new HourWeather
                    {
                        Time = weathers[i].Time.Split(" ")[1],
                        Temp = double.Parse(weathers[i].Temp),
                        TempMax = weathers[i].MaxTemp,
                        TempMin = weathers[i].MinTemp,
                    });
                }

                HourWeatherList = new ObservableCollection<HourWeather>(weatherList.GroupBy(t => t.Time).Select(g => g.First()).ToList());
            }
        }

    }
}
