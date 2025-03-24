using AppWeather.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using System.Xml.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using IL2CPPToWinRTBridge;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.Text;
using System.Threading;
using Windows.UI.Xaml.Controls.Primitives;
using System.IO;
using Newtonsoft.Json;
using System.Diagnostics;
using Windows.UI.Core;
using Microsoft.Graphics.Canvas.Effects;
using Windows.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls;


namespace AppWeather.ViewModels
{
    public class HomePageViewModel : ObservableObject
    { 
        //public string cityName = "Shanghai";
        public List<WeatherClass> weathers;
        public BackgroundWorker bgWorker = null;
        public ObservableCollection<EveryDayWeather> everyDayWeathers = new ObservableCollection<EveryDayWeather>();
        public bool IsInitialized { get; set; }

        //private V3WxObservationsCurrent currentWeatherCn;//固定语言版本的当前天气，用于和Untity通信；-还是用中文吧

        //public V3WxObservationsCurrent CurrentWeatherCn 
        //{
        //    get { return currentWeatherCn; }
        //    set { SetProperty(ref currentWeatherCn, value); }
        //}

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

        private double _degPressure;
        /// <summary>
        /// 浮点数的气压,将气压从300-1700转换为0-360
        /// </summary>
        public double degPressure
        {
            get => _degPressure;
            set => SetProperty(ref _degPressure, value);
        }

        private double _dbPressure;

        /// <summary>
        /// 浮点数的气压,单位hPa
        /// </summary>
        public double dbPressure
        {
            get => _dbPressure;
            set => SetProperty(ref _dbPressure, value);
        }

        double Foo1(double inputValue)
        {
            //将300-1700转换为0-360
            if (inputValue >= 300.0 && inputValue <= 1700.0)
                return ((inputValue - 300.0) / ((1700.0 - 300.0) / 360.0));
            return 0.0;
        }

        private string _pressure;

        /// <summary>
        /// 气压
        /// </summary>
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
        private double _windDegree1;
        /// <summary>
        /// 这是风向啊~~~~~~~~~~
        /// </summary>
        public double WindDegree
        {
            get => _windDegree;
            set { SetProperty(ref _windDegree, value);
                WindDegree1=value+40;
            }
            
        }

        public double WindDegree1
        {
            get => _windDegree1;
            set => SetProperty(ref _windDegree1, value);

        }

        string uvDescription;
        public string UvDescription
        {
            get { return uvDescription; }
            set {  SetProperty(ref uvDescription, value);}
        }

        int uvIndex;
        public int UVIndex
        {
            get { return uvIndex; }
            set { SetProperty(ref uvIndex, value); }
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

        ApplicationDataContainer GetLocationSettings { get { return Windows.Storage.ApplicationData.Current.LocalSettings; } }
 

        public ICommand RefreshCommand { get; }
        private WeatherClass CurrentWeather { get;  set; }

        public HomePageViewModel()
        {
            LocationPageViewModel.RefreshHomePage += Refresh;
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

            //Task.Run(async() => {
            //    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    server.Bind(new IPEndPoint(IPAddress.Any, 2333));//监听本地回环地址2333端口
            //    while (true)
            //    {
            //        try
            //        {
            //            server.Listen(10);
            //            var Accept = server.Accept();
            //            while(Accept.Connected)
            //            {
            //                var buffer = Encoding.UTF8.GetBytes("Hello");
            //                Accept.Send(buffer);
            //                await Task.Delay(100);
            //            }
            //        }
            //        catch (Exception)
            //        {
            //            //throw;
            //        }
            //    }
            //});
        }

 

        private void Refresh()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            String homeCity = localSettings.Values["Location"] as string;

            //if (!String.IsNullOrEmpty(homeCity))
            //    cityName = homeCity;
            //else
            //    //set default value
            //    cityName = "Shanghai";



            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += refreshWeatherBW_DoWork;//工作线程回调，将要执行的代码放在此函数里    
            bgWorker.RunWorkerCompleted += refreshWeatherBW_RunWorkerCompleted;//当完成时回调
            ProgressRingVisbility = Visibility.Visible;
            IsBusy = true;
            bgWorker.RunWorkerAsync();
        }

        public void Initialize()
        {
            // Initialization code...
            Refresh();
            IsInitialized = true;
        }

        private void refreshWeatherBW_DoWork(object sender, DoWorkEventArgs e)
        {
            //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //localSettings.Values["LocationPlaceId"] = WeatherAdapter.GetLocationInformation("漠河市").location[0].placeId; 
            var placeId = GetLocationSettings.Values["LocationPlaceId"] as string;

            //var localCityName= new GetLocationCityName().GetLocalCityName().GetAwaiter().GetResult();
            //var tmp= WeatherAdapter.GetLocationInformation(localCityName)?.location[0].placeId;
            if (string.IsNullOrWhiteSpace(placeId))
            {
                var cityName = new GetLocationCityName().GetLocalCityName().GetAwaiter().GetResult();
                placeId = WeatherAdapter.GetLocationInformation(cityName)?.location[0].placeId;
            }
            //weathers = OpenWeather.GetWeathers(cityName);
            weathers = WeatherAdapter.GetWeathers(placeId);

            //CurrentWeatherCn = WeatherAdapter.GetCurrentWeater(placeId, "zh-CN");//获取固定语言的天气信息，用于判断天气图标以及和Untity通信            
        }

        private async Task WriteMessage(string msg)
        {
            // 捕获并处理异常
            try
            {
                // 获取应用程序的本地存储文件夹
                StorageFolder localFolder = ApplicationData.Current.LocalFolder;

                // 在后台线程中创建文件
                await Task.Run(async () =>
                {
                    // 创建文件
                    StorageFile sampleFile = await localFolder.CreateFileAsync("sample.txt", CreationCollisionOption.ReplaceExisting);

                    // 在文件中写入内容
                    await FileIO.WriteTextAsync(sampleFile, msg);

                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"写入文件时出错: {ex.Message}");
            }
        }

        string WriteMsg(string msg)
        {
            try
            {
                string path = "";


                StorageFile file;

                //var tmpFolder = ApplicationData.Current.TemporaryFolder;
                //file = tmpFolder.CreateFileAsync("MyData.txt", CreationCollisionOption.ReplaceExisting).AsTask().GetAwaiter().GetResult();

                var curFolder = ApplicationData.Current.LocalFolder;
                file = curFolder.CreateFileAsync(@"StreamingAssets\WeatherData.txt", CreationCollisionOption.ReplaceExisting).AsTask().GetAwaiter().GetResult();

                FileIO.WriteTextAsync(file, msg).AsTask().GetAwaiter().GetResult();
                Debug.WriteLine($"输出文件完整路径: {file.Path}");

                return file.Path;
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
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
                    CurrentWeather = weathers.OrderBy(x => Math.Abs((long)x.TimeStamp - time)).First(); //weathers[index];
                    //LocationName = cityName;
                    LocationName = weathers.FirstOrDefault().CityName;
                    //Weather = CurrentWeatherCn.cloudCoverPhrase;//CurrentWeather.State;
                    Tip = "";
                    //？
                    //if (Weather.Equals("Clouds"))
                    //{
                    //    CurrentWeatherImg = "../Images/Cloud.png";

                    //}
                    //else if (Weather.Equals("Rain"))
                    //{
                    //    CurrentWeatherImg = "../Images/Rainny.png";
                    //}
                    //else if (Weather.Equals("Clear"))
                    //{
                    //    CurrentWeatherImg = "../Images/Clear.png";
                    //}
                    //File.WriteAllText("Weathers.txt", CurrentWeather.IconCode.ToString());
                    var moonPhase = CurrentWeather.MoonPhase;
                    var moonPhaseCode = CurrentWeather.MoonPhaseCode;
                    var moonPhaseDay= CurrentWeather.MoonPhaseDay;

                    //Send(CurrentWeather.IconCode.ToString());//直接把中文的气象状态发给Untity；

                    Debug.WriteLine(WriteMsg(CurrentWeather.IconCode.ToString()+","+moonPhaseDay +"," + CurrentWeather.DayOrNight));                    

                    //window.chrome.webview.postMessage

                    // Create sample file; replace if exists.
                    //Windows.Storage.StorageFolder storageFolder =
                    //    Windows.Storage.ApplicationData.Current.LocalFolder;
                    //Windows.Storage.StorageFile sampleFile =
                    //    storageFolder.CreateFileAsync("sample.txt",
                    //        Windows.Storage.CreationCollisionOption.ReplaceExisting).GetResults();

                    //Windows.Storage.FileIO.WriteTextAsync(sampleFile, JsonConvert.SerializeObject(CurrentWeather.IconCode)).GetResults();
                    //Debug.WriteLine(Windows.Storage.FileIO.ReadTextAsync(sampleFile).GetResults());

                    //WriteMessage("1ekhkwerwrw").GetAwaiter().GetResult();    


                    //var tempFolder = ApplicationData.Current.TemporaryFolder;

                    //Task.Run(() => {
                    //    var tempFile = tempFolder.CreateFileAsync("temp.txt", CreationCollisionOption.ReplaceExisting).GetResults();
                    //    FileIO.WriteTextAsync(tempFile, "12397982497297984").GetResults();
                    //}).GetAwaiter().GetResult();

                    //Debug.WriteLine(FileIO.ReadTextAsync(tempFile).GetResults());

                    //if(Weather.Contains("雨"))
                    //{ 
                    //    CurrentWeatherImg = "../Images/Rainny.png";
                    //}
                    //else if(Weather.Contains("晴朗"))
                    //{ 
                    //    CurrentWeatherImg = "../Images/Clear.png";
                    //}
                    //else if(Weather.Contains("雪"))
                    //{ 
                    //    CurrentWeatherImg = "../Images/Cloud.png";
                    //}
                    //else
                    //{ 
                    //    CurrentWeatherImg = "../Images/Cloud.png";
                    //}

                    //CurrentWeatherImg = $"/WeatherIcons/{CurrentWeatherCn.iconCode}.png";

                    //当前温度
                    double temp = double.Parse(CurrentWeather.Temp);
                    CurrentTemp = temp.ToString("0.0") + "℃";

                    FeelsLike = CurrentWeather.FeelsLike.ToString("0.0") + "℃";//体感温度
                    //今日最低最高温度 
                    double todayMinTemp;//= weathers.Where(i => DateTime.Parse(i.Time).Date==DateTime.Today.Date).Min(k => k.MinTemp);
                    double todayMaxTemp;//= weathers.Where(i => DateTime.Parse(i.Time).Date == DateTime.Today.Date).Max(k => k.MaxTemp);

                    //获取24小时内的最低温度和最高温度
                    todayMinTemp = weathers.OrderBy(k => k.Time).Take(24).Min(j=>j.MinTemp);
                    todayMaxTemp = weathers.OrderBy(k => k  .Time).Take(24).Max(j => j.MaxTemp);
                    TodayTemperature = todayMinTemp.ToString("0.0") + "℃-" + todayMaxTemp.ToString("0.0") + "℃";

                    Humidity = CurrentWeather.Humidity.ToString();
                    //他说用这个
                    dbPressure = CurrentWeather.Pressure;//浮点值的气压单位hPa
                    degPressure = Foo1(CurrentWeather.Pressure);//角度值气压，范围0-360
                    Pressure = CurrentWeather.Pressure.ToString();
                    Vsibility = CurrentWeather.Vsibility.ToString("0.0") + "km";
                    WindSpeed = CurrentWeather.WindSpeed.ToString();
                    //WindDegree = CurrentWeather.WindDegree;
                    WindDegree = CurrentWeather.WindDegree + 90;

                    UvDescription = CurrentWeather.UVDescription;
                    UVIndex = CurrentWeather.UvIndex;

                    Rainfall = CurrentWeather.Rainfall.ToString("0.0") + "%";
                    System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));//当地时区
                    Sunrise = startTime.AddSeconds(CurrentWeather.Sunrise).ToLongTimeString().ToString();
                    Sunset = startTime.AddSeconds(CurrentWeather.Sunset).ToLongTimeString().ToString();

                    everyDayWeathers.Clear();

                    var tMinTemps = weathers.GroupBy(a => DateTime.Parse(a.Time).Date).Select(b => b.Min(c => c.MinTemp)).ToArray();
                    var tMaxTemps = weathers.GroupBy(a => DateTime.Parse(a.Time).Date).Select(b => b.Max(c => c.MaxTemp)).ToArray();
                    //显示6日天气
                    for (int i = 1; i < 9; i++)
                    {
                        //string ttime = DateTime.Now.AddDays(i).ToString("u");
                        //string tDate = ttime.Split(' ')[0];
                        string weather = weathers.Where(k => DateTime.Parse(k.Time).Date == (DateTime.Now.AddDays(i)).Date).FirstOrDefault().State;
                        string img = "\uE9BD";
                        //if (weather.Equals("Clouds"))
                        //{
                        //    img = "\uE9BF";
                        //}
                        //else if (weather.Equals("Rain"))
                        //{
                        //    img = "\uE9E2";
                        //}
                        //else if (weather.Equals("Sunny"))
                        //{
                        //    img = "\uE9BD";
                        //}

                        //直接用天气的Code判断图标
                        //if(weather.Contains("雨")) img = "\uE9E2";
                        //else if(weather.Contains("晴朗")) img = "\uE9BD";
                        //else img = "\uE9BF";
                        img = $"/WeatherIcons/{weathers.Where(k => DateTime.Parse(k.Time).Date == (DateTime.Now.AddDays(i)).Date).FirstOrDefault().IconCode}.png";

                        double tMinTemp = weathers.Where(k => DateTime.Parse(k.Time).Date == (DateTime.Now.AddDays(i)).Date).Min(k => k.MinTemp);
                        double tMaxTemp = weathers.Where(k => DateTime.Parse(k.Time).Date == (DateTime.Now.AddDays(i)).Date).Max(k => k.MaxTemp);

                        tMinTemp = tMinTemps[i];
                        tMaxTemp = tMaxTemps[i];
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

        // UWP 发送消息
        public static bool Send(string message)
        {
            IIL2CPPBridge bridge = BridgeBootstrapper.GetIL2CPPBridge();
            // 如果同一时间在Unity中IIL2CPPBridge已经实例化(即上文的初始化),则不为null
            if (bridge != null)
            {
                bridge.Connect(message);
                return true;
            }
            return false;
        }

    }
}
