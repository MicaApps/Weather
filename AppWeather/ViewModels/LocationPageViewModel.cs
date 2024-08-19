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
using System.Diagnostics;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Controls;
using System.IO;
using Newtonsoft.Json;
using System.Data.SQLite;
using System.Globalization;
using System.Net.Http;
using Windows.UI.Core;
using System.Threading;
using System.Text.RegularExpressions;

namespace AppWeather.ViewModels
{
    public class LocationPageViewModel : ObservableObject
    {
        BackgroundWorker bgWorker = null;

        public static event Action RefreshHomePage;       

        //public ObservableCollection<SimpleWeatherClass> cityWeathers = new ObservableCollection<SimpleWeatherClass>();        

        private ObservableCollection<SimpleWeatherClass> _cityWeathers=new ObservableCollection<SimpleWeatherClass>();
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

        private Visibility _cityViewVisbility;

        public Visibility CityViewVisbility
        {
            get => _cityViewVisbility;
            set => SetProperty(ref _cityViewVisbility, value);
        }

        public ICommand RefreshCommand { get; }
        public ICommand SetAsMainCommand { get; }
        public IAsyncRelayCommand ClearCommand { get; }
        public ICommand DeleteCommand { get; }
        public IAsyncRelayCommand AddCommand { get; }

        public ICommand ItemClickCommand { get; }


        public LocationPageViewModel()
        {
            RefreshCommand = new RelayCommand(Refresh);
            SetAsMainCommand = new RelayCommand(SetAsMainCity);
            ClearCommand = new AsyncRelayCommand(ClearCities);
            DeleteCommand = new RelayCommand(DeleteCity);
            AddCommand = new AsyncRelayCommand<Page>(AddCity);
            ItemClickCommand = new RelayCommand(ItemClickHandler);

        }

        private void ItemClickHandler()
        {

        }


        private void Refresh()
        {
            ProgressRingVisbility = Visibility.Visible;
            IsBusy = true;
            LoadCity();
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += LoadCityBW_DoWork;
            bgWorker.RunWorkerCompleted += AddCityBW_RunWorkerCompleted;//当完成时回调
            bgWorker.RunWorkerAsync();
        }

        private void SetAsMainCity()
        {
            
            //此处无法获取选取城市的PlaceId
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (SelectedCity != null)
            {
                var weatherinfo = SelectedCity as SimpleWeatherClass;

               

                // load a setting that is local to the device
                //localSettings.Values["Location"] = weatherinfo.CityName;
                localSettings.Values["LocationPlaceId"] = SelectedCity.PlaceId;// weatherinfo.PlaceId;
                RefreshHomePage?.Invoke();
            } 
        }

        private void DeleteCity()
        {
            if (SelectedCity != null)
            {
                var weatherinfo = SelectedCity as SimpleWeatherClass;
                string cityName = weatherinfo.CityName;
                int index = CityWeathers.ToList().FindIndex(x => x.CityName.Equals(cityName));
                if (index != -1)
                {
                    ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                    // load a setting that is local to the device
                    String cities = localSettings.Values["Cities"] as string;
                    List<string> cityArray = cities.Split('&').ToList();
                    cityArray.Remove(cityName);
                    localSettings.Values["Cities"] = string.Join('&', cityArray);

                    var placeIdList = localSettings.Values["placeId"].ToString().Split("&").ToList();
                    placeIdList.Remove(weatherinfo.PlaceId);

                    localSettings.Values["placeId"] =string.Join("&", placeIdList);
                    CityWeathers.Remove(weatherinfo);
                }
            }
        }

        private async Task ClearCities()
        {
            ContentDialog cd = new ContentDialog() { Title = "AskClear", Content = "AllClear" };
            cd.PrimaryButtonText = "OK";
            cd.PrimaryButtonClick += (ss, ee) =>
            {
                CityWeathers.Clear();//清空缓存的城市列表
                ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;//载入配置文件
                localSettings.Values["Cities"] = string.Empty;//清空配置文件中的Cities字段内容
                localSettings.Values["placeId"] = string.Empty;//清空配置文件中的Cities字段内容
            };
            cd.CloseButtonText = "Cancle";
            await cd.ShowAsync();//刷新界面
        }        

        //string GetApiAddress(string CityName)
        //{
        //    string cultureInfo = CultureInfo.CurrentCulture.Name.ToLower();
        //    return $"https://api.weather.com/v3/location/searchflat?query={CityName}&language={cultureInfo}&apiKey=793db2b6128c4bc2bdb2b6128c0bc230&format=json";
        //}

        //string GetApiAddress(string CityName,string cultureInfo)
        //{ 
        //    return $"https://api.weather.com/v3/location/searchflat?query={CityName}&language={cultureInfo}&apiKey=793db2b6128c4bc2bdb2b6128c0bc230&format=json";
        //}

        private async Task AddCity1(Page _page)
        {
            //To-Do:获取城市的Json数据
            //下面这个Json数据太多，读取会直接卡死
            
            var path = System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json";
            string dbFileName = System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\CityList.db";            

            SQLiteConnection conn = new SQLiteConnection($"Data Source={dbFileName};Version=3");
            SQLiteCommand cmd = new SQLiteCommand("", conn);
            conn.Open();

            HttpClient client = new HttpClient();
            // string striparr = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json");
            // string striparr = "[\n    {\n        \"id\": 833,\n        \"name\": \"Ḩeşār-e Sefīd\",\n        \"state\": \"\",\n        \"country\": \"IR\",\n        \"coord\": {\n            \"lon\": 47.159401,\n            \"lat\": 34.330502\n        }\n    },\n    {\n        \"id\": 2960,\n        \"name\": \"‘Ayn Ḩalāqīm\",\n        \"state\": \"\",\n        \"country\": \"SY\",\n        \"coord\": {\n            \"lon\": 36.321911,\n            \"lat\": 34.940079\n        }\n    },\n    {\n        \"id\": 3245,\n        \"name\": \"Taglag\",\n        \"state\": \"\",\n        \"country\": \"IR\",\n        \"coord\": {\n            \"lon\": 44.98333,\n            \"lat\": 38.450001\n        }\n    },\n    {\n        \"id\": 3530,\n        \"name\": \"Qabāghlū\",\n        \"state\": \"\",\n        \"country\": \"IR\",\n        \"coord\": {\n            \"lon\": 46.168499,\n            \"lat\": 36.173302\n        }\n    },]";

            List <CityInfo> cities = null;

            

            //cities = await Task.Run(() => {
            //    var mem = new MemoryStream();
            //    mem.SetLength(new FileInfo(path).Length);
            //    using (var f = File.OpenRead(path))
            //    {
            //        f.CopyTo(mem);
            //    }
            //    // var text = System.IO.File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json");
            //    var serializer = new JsonSerializer();
            //    var text = Encoding.UTF8.GetString(mem.ToArray());
            //    return JsonConvert.DeserializeObject<List<CityInfo>>(text);
            //});


            // using (var f = File.OpenRead(System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json"))
            //using (var f = File.Open(@"D:\Users\Shomn\OneDrive - MSFT\Downloads\Weather-DESKTOP-8O1AG92\Weather\AppWeather\Models\DataBase\city.list.json", FileMode.Open))
            //using (StreamReader sr = new StreamReader(f))
            // using (JsonReader reader = new JsonTextReader(sr))
            //{
            //    while (reader.Read())
            //    {
            //        // deserialize only when there's "{" character in the stream
            //        //if (reader.TokenType == JsonToken.StartArray)
            //        {
            //            JsonSerializer serializer = new JsonSerializer();

            //            cities = serializer.Deserialize(reader, typeof(List<CityInfo>)) as List<CityInfo>;
            //            // cities = o as List<CityInfo>;

            //            // cities = serializer.Deserialize<MyObject>(reader);
            //        }
            //    }
            //}
            // Debug.WriteLine(striparr);
            // /*List<CityInfo>*/ cities = JsonHelper.DeserializeJsonToList<CityInfo>(striparr);

            //List<string> cityStrList = cities.Select(t => t.name).ToList();

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            var textBox = new AutoSuggestBox() { Width = 200, Height = 32, VerticalAlignment = VerticalAlignment.Center };
            string cityName = "";
            //TextBox textBox = new TextBox() { Width = 200, Height = 32, VerticalAlignment = VerticalAlignment.Center };
            //ComboBox cityComboBox = new ComboBox() { Height = 32, VerticalAlignment = VerticalAlignment.Center };
            //textBox.LostFocus += (s1, e1) =>
           

            textBox.TextChanged += async (s1, e1) =>
            {
                try
                {
                    if (e1.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                    {
                        
                        var cityNameList = new ObservableCollection<string>();

                        if (string.IsNullOrWhiteSpace(textBox.Text)) return;

                        cityName = textBox.Text.Trim().ToLower();

                        cmd = new SQLiteCommand("", conn);
                        cmd.CommandText = $"SELECT name FROM CityList WHERE name LIKE @searchTerm;";
                        cmd.Parameters.AddWithValue("@searchTerm", $"%{cityName}%");

                        var rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            cityNameList.Add(rd.GetString(0));
                            if (cityNameList.Count > 10) break;//只添加10个
                        }
                        rd.Close();
                        //至此必定添加完成
                        //cityComboBox.ItemsSource = cityNames;

                        textBox.ItemsSource = cityNameList; 
                    }                   

                }
                catch(Exception ex)
                {
                    //throw;
                }
            };
            // 选择建议时触发
            textBox.SuggestionChosen += (AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args) =>
             {
                 var selectedItem = args.SelectedItem;
                 if (selectedItem != null)
                 {
                     textBox.Text = selectedItem.ToString().Split("-").FirstOrDefault();
                 }
             };


            //cityComboBox.ItemsSource = cityStrList;
            panel.Children.Add(textBox);
            //panel.Children.Add(cityComboBox);

            //cityComboBox.SelectedIndex = 0;
            ContentDialog cd = new ContentDialog()
            {
                Content = panel,
                Title = "Add City",
            };
            cd.PrimaryButtonText = "OK";
            cd.PrimaryButtonClick += async (s1, e1) =>
            {
                //if (cityComboBox.SelectedIndex > -1)
                if(!string.IsNullOrEmpty(cityName))
                {
                    //再查询一遍，若数据库中没有结果就跳出
                    cmd = new SQLiteCommand("", conn);
                    cmd.CommandText = $"SELECT COUNT(*) FROM CityList WHERE name = ?;";
                    //cmd.Parameters.AddWithValue("@searchTerm", $"%{textBox.Text}%");
                    cmd.Parameters.Add(new SQLiteParameter("param", textBox.Text));
 
                    if (((Int64)cmd.ExecuteScalar()) > 0)
                    {
                        //CityView.Visibility = Visibility.Collapsed;
                        bgWorker = new BackgroundWorker();
                        bgWorker.DoWork += AddCityBW_DoWork;
                        bgWorker.RunWorkerCompleted += AddCityBW_RunWorkerCompleted;//当完成时回调
                        ProgressRingVisbility = Visibility.Visible;
                        IsBusy = true;
                        //bgWorker.RunWorkerAsync(cities[cityComboBox.SelectedIndex].name);
                        bgWorker.RunWorkerAsync(textBox.Text);

                        CityWeathers.Add(OpenWeather.GetSimpleWeather(textBox.Text));//向UI中添加城市-天气和温度需要调用Api获取

                        SaveCityList();//保存城市列表
                    }
                    else
                    {
                        //throw new Exception("错误的输入！");
                    } 
                    
                }
            };
            cd.CloseButtonText = "Cancle";

            cd.XamlRoot = _page.XamlRoot;
            await cd.ShowAsync();

        }

        private async Task AddCity(Page _page)
        {
            //HttpClient client = new HttpClient();
            List<CityInfo> cities = null;

            StackPanel panel = new StackPanel() { Orientation = Orientation.Horizontal };
            var textBox = new AutoSuggestBox() { Width = 200, Height = 32, VerticalAlignment = VerticalAlignment.Center };
            textBox.UpdateTextOnSelect = false;
            string cityName = "";
            //TextBox textBox = new TextBox() { Width = 200, Height = 32, VerticalAlignment = VerticalAlignment.Center };
            //ComboBox cityComboBox = new ComboBox() { Height = 32, VerticalAlignment = VerticalAlignment.Center };
            //textBox.LostFocus += (s1, e1) =>

            textBox.TextChanged += async (s1, e1) =>
            {
                try
                {
                    if (e1.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
                    {
                        var cityNameList = new ObservableCollection<CityInfo>();
                        if (string.IsNullOrWhiteSpace(textBox.Text)) return;
                        cityName = textBox.Text.Trim().ToLower();
                        await Task.Run(() => {
                            var citys = WeatherAdapter.GetLocationInformation(cityName);
                            citys.location.ForEach(item =>
                            {
                                if (!string.IsNullOrWhiteSpace(item.city)) cityNameList.Add(new CityInfo() { CityName = item.displayName, Location = item.country });
                            });
                        });

                        textBox.ItemsSource = cityNameList; 
                    }
                }
                catch (Exception ex)
                {
                    //throw;
                }
            };
            // 选择建议时触发 
            textBox.SuggestionChosen += async (AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)=>
            { 
                await Task.Delay(200);
                var selectedItem = args.SelectedItem;
                if (selectedItem != null)
                {
                    var str = (((CityInfo)selectedItem).CityName);
                    textBox.Text = str;//ToString().Split("-").FirstOrDefault();
                }
            };
            //cityComboBox.ItemsSource = cityStrList;
            panel.Children.Add(textBox);
            //panel.Children.Add(cityComboBox);

            //cityComboBox.SelectedIndex = 0;
            ContentDialog cd = new ContentDialog()
            {
                Content = panel,
                Title = "Add City",
            };
            cd.PrimaryButtonText = "OK";
            cd.PrimaryButtonClick += async (s1, e1) =>
            {
                cityName=textBox.Text.Trim();
                if (!string.IsNullOrEmpty(cityName))
                {
                    //此时TextBox.Text已被替换
                    var locationData = WeatherAdapter.GetLocationInformation(cityName);
                    if (locationData.location.Any(x => x.displayName.Equals(cityName)))
                    {
                        bgWorker.RunWorkerCompleted += AddCityBW_RunWorkerCompleted;//当完成时回调
                        ProgressRingVisbility = Visibility.Visible;
                        IsBusy = true;
                        bgWorker.RunWorkerAsync(cityName);
                        CityInfo cityInfo = new CityInfo();                                                                                                                                                                                 //北京，晴朗，36.1这样的格式
                        var placeId = WeatherAdapter.GetLocationInformation(cityName).location[0].placeId;
                        AddPlaceId(placeId);
                        var weather = WeatherAdapter.GetSimpleWeater(placeId);
                        if (!CityWeathers.Any(a=>a.CityName.Equals(weather.CityName))) CityWeathers.Add(weather);//向UI中添加城市-天气和温度需要调用Api获取
                    }
                    else
                    {
                        //throw new Exception("错误的输入！");
                    }
                }
            };
            cd.CloseButtonText = "Cancle";

            cd.XamlRoot = _page.XamlRoot;
            await cd.ShowAsync();
        }

        public partial class CityInfo:ObservableObject
        { 
            string cityName; 
            string location;

            public string CityName 
            {
                get { return cityName; }
                set { SetProperty(ref cityName, value); }
            }

            public string Location
            {
                get { return location; }
                set { SetProperty(ref  location, value); }
            }

            public override string ToString()
            {
                return CityName + "-" + Location;
            }
        }

        //初始化时从配置文件内载入城市数据
        private async void LoadCityBW_DoWork(object sender, DoWorkEventArgs e)
        {
            //LoadCity();
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            // load a setting that is local to the device
            //从配置文件中载入Cities这个属性然后查询天气内容
            String cities = localSettings.Values["Cities"] as string;
            if (cities != null)
            {
                string[] cityArray = cities.Split('&');

                foreach (var city in cityArray)
                {
                    if (!string.IsNullOrEmpty(city))
                    {
                        //SimpleWeatherClass weather = OpenWeather.GetSimpleWeather(city);
                        //AddCityData(weather);
                    } 
                }
            }
        }

        private void LoadCity()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings; 
            //从配置文件中载入Cities这个属性然后查询天气内容
            String placeIds = localSettings.Values["placeId"] as string;
            if (placeIds != null)
            {
                string[] cityArray = placeIds.Split('&');

                foreach (var id in cityArray)
                {
                    if (!string.IsNullOrEmpty(id))
                    {
                        var weather = WeatherAdapter.GetSimpleWeater(id);
                        if (CityWeathers.Any(a => a.CityName.Equals(weather.CityName))) return;
                        CityWeathers.Add(weather);
                        //SimpleWeatherClass weather = OpenWeather.GetSimpleWeather(city);
                        //AddCityData(weather);
                    }
                }
            }
        }


        public async Task AddCityData(SimpleWeatherClass weather)
        {            
            //if (!cityWeathers.Any(x => x.CityName.Equals(weather.CityName))) cityWeathers.Add(weather);
            //return Task.Run(() =>
            //{ 
            //    if (weather != null)
            //    {
            //        if (weather.CityName != null)
            //        {
            //            //if (!cityWeathers.Any(x => x.CityName.Equals(weather.CityName))) cityWeathers.Add(weather);

            //            //index = cityWeathers.ToList().FindIndex(x => x.CityName.Equals(weather.CityName));
            //            //if (index == -1)
            //            //{
            //            //    //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            //            //    //        () =>
            //            //    //        {
            //            //                //this part is being executed back on the UI thread...

            //            //            //});

            //            //}
            //            //else
            //            //{
            //            //    //await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal,
            //            //    //        () =>
            //            //    //        {
            //            //                //this part is being executed back on the UI thread...
            //            //                cityWeathers[index] = weather;
            //            //            //});
            //            //    ////更新现有信息

            //            //}
            //        }                    
            //    }                
            //});
            
        }

        private async void AddCityBW_DoWork(object sender, DoWorkEventArgs e)
        {
            //string cityName = e.Argument.ToString();

            //ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            //string cities = localSettings.Values["Cities"] as string;
            //if (cities == null)
            //{
            //    cities = string.Empty;
            //}

            //List<string> cityList = cities.Split('&').ToList();
            //cityList.Add(cityName);
            //localSettings.Values["Cities"] = string.Join('&', cityList); 
            



            //SimpleWeatherClass weather = OpenWeather.GetSimpleWeather(cityName);//问题处在这里
            //AddCityData(weather);                      
            //CityWeathers.Add(new SimpleWeatherClass { CityName = cityName, State = "", Temp = 0.0 });//向UI中添加城市
        }

        void SaveCityList()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["Cities"] = String.Join('&',CityWeathers.Select(a=>a.CityName).ToArray());
            
        }

        void AddPlaceId(string placeId)
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string placeIds = localSettings.Values["placeId"] as string;
            if (placeIds == null)
            {
                placeIds = string.Empty;
            }

            List<string> placeIdList = placeIds.Split('&').ToList();
            placeIdList.Add(placeId);
            localSettings.Values["placeId"] = string.Join('&', placeIdList);
        }

        private void AddCityBW_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                CityViewVisbility = Visibility.Visible;
                ProgressRingVisbility = Visibility.Collapsed;
                IsBusy = false;
                CityWeathers = CityWeathers;//UI中绑定的是CityWeather这个属性
               
                //CityWeathers.Add(new SimpleWeatherClass { CityName = "", State = "", Temp = 0.0 });//向UI中添加城市

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            //cityWeathers.Clear();
            CityWeathers.Clear();
            CityWeathers = null;
        }



        public void CityView_RightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var wather = (e.OriginalSource as FrameworkElement).DataContext as SimpleWeatherClass;
        }
    }
}
