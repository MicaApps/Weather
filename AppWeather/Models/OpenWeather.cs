using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace AppWeather.Models
{
    public class OpenWeather
    {
        public static string appid = "6d7f5f935437b4c1440b9b95f0b301f7";
        //string url = "http://api.openweathermap.org/data/2.5/forecast?id=1796236&appid=6d7f5f935437b4c1440b9b95f0b301f7";

        public static string citylistjson =
                File.ReadAllText(System.AppDomain.CurrentDomain.BaseDirectory + @"\Models\DataBase\city.list.json");
        public static List<CityInfo> cityInfos;


        public static RootObject GetWeather(string cityName)
        {
            if (cityInfos == null)
                cityInfos = JsonHelper.DeserializeJsonToList<CityInfo>(citylistjson);

            CityInfo cityinfo = cityInfos.Where(city => city.name.Equals(cityName)).FirstOrDefault();

            string cityID = ((int)cityinfo.id).ToString();
            string url = "http://api.openweathermap.org/data/2.5/forecast?id=" + cityID + "&appid=" + appid;

            string response = request(url);
            string result = Regex.Unescape(response);
            RootObject rootObject = JsonHelper.DeserializeJsonToObject<RootObject>(result);

            return rootObject;
        }

        public static string request(string url)
        {
            //System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)WebRequest.Create(url);
            //request.Method = "GET";
            //System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)request.GetResponse();
            //System.IO.Stream s = response.GetResponseStream();
            //GZipStream gZipStream = new GZipStream(s, CompressionMode.Decompress);
            //StreamReader Reader = new StreamReader(gZipStream, Encoding.Unicode);
            //string StrDate = Reader.ReadLine();
            //return StrDate;
            try
            {
                string json = "{\"cod\":\"404\",\"message\":\"city not found\"}";
                using (WebClient wc = new WebClient())
                {
                    json = wc.DownloadString(url);
                }
                return json;
            }
            catch (System.Net.WebException ex)
            {
                return "{\"cod\":\"404\",\"message\":\"city not found\"}";
            }

        }

        public static List<WeatherClass> GetWeathers(string cityName)
        {
            List<WeatherClass> weathers = new List<WeatherClass>();

            RootObject weatherInfo = OpenWeather.GetWeather(cityName);
            if (weatherInfo != null)
            {
                if (!weatherInfo.cod.Equals("404"))
                {
                    for (int i = 0; i < weatherInfo.list.Count; i++)
                    {
                        weathers.Add(new WeatherClass
                        {
                            CityName = cityName,
                            State = weatherInfo.list[i].weather[0].main,
                            Temp = (double.Parse(weatherInfo.list[i].main.temp) - 273.15).ToString("0.0"),
                            FeelsLike = double.Parse(weatherInfo.list[i].main.feels_like) - 273.15,
                            MinTemp = double.Parse(weatherInfo.list[i].main.temp_min) - 273.15,
                            MaxTemp = double.Parse(weatherInfo.list[i].main.temp_max) - 273.15,
                            Humidity = double.Parse(weatherInfo.list[i].main.humidity),
                            Pressure = double.Parse(weatherInfo.list[i].main.pressure),
                            Vsibility = double.Parse(weatherInfo.list[i].visibility) / 1000,
                            WindSpeed = double.Parse(weatherInfo.list[i].wind.speed),
                            WindDegree = double.Parse(weatherInfo.list[i].wind.deg),
                            Rainfall = 100 - double.Parse(weatherInfo.list[i].clouds.all),
                            Time = weatherInfo.list[i].dt_txt,
                            TimeStamp = long.Parse(weatherInfo.list[i].dt),
                            Sunrise = long.Parse(weatherInfo.city.sunrise),
                            Sunset = long.Parse(weatherInfo.city.sunset),
                        });

                        System.Diagnostics.Debug.WriteLine("State：" + weatherInfo.list[i].weather[0].main +
                                                            "\nMinTemp：" + (double.Parse(weatherInfo.list[i].main.temp_min) - 273.15).ToString() +
                                                            "\nMaxTemp：" + (double.Parse(weatherInfo.list[i].main.temp_max) - 273.15).ToString() +
                                                            "\nTime：" + weatherInfo.list[i].dt_txt);
                    }

                }
                else
                {
                    weathers = null;
                }
            }
            else
            {
                weathers = null;
            }

            return weathers;
        }

        public static SimpleWeatherClass GetSimpleWeather(string cityName)
        {
            List<SimpleWeatherClass> weathers = new List<SimpleWeatherClass>();
            SimpleWeatherClass currentSimpleWeather = new SimpleWeatherClass();

            RootObject weatherInfo = OpenWeather.GetWeather(cityName);
            if (weatherInfo != null)
            {
                if (!weatherInfo.cod.Equals("404"))
                {
                    for (int i = 0; i < weatherInfo.list.Count; i++)
                    {
                        weathers.Add(new SimpleWeatherClass
                        {
                            CityName = cityName,
                            State = weatherInfo.list[i].weather[0].main,
                            Temp = double.Parse(weatherInfo.list[i].main.temp) - 273.15,
                            MinTemp = double.Parse(weatherInfo.list[i].main.temp_min) - 273.15,
                            MaxTemp = double.Parse(weatherInfo.list[i].main.temp_max) - 273.15,
                            TimeStamp = long.Parse(weatherInfo.list[i].dt),
                        });
                    }

                    long currenTime = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
                    currentSimpleWeather = weathers.OrderBy(x => Math.Abs((long)x.TimeStamp - currenTime)).First();

                }
                else
                {
                    currentSimpleWeather = null;
                }
            }
            else
            {
                currentSimpleWeather = null;
            }

            return currentSimpleWeather;
        }
    }

    [Serializable]
    public class Main
    {
        public string temp { get; set; }
        public string feels_like { get; set; }
        public string temp_min { get; set; }
        public string temp_max { get; set; }
        public string pressure { get; set; }
        public string sea_level { get; set; }
        public string grnd_level { get; set; }
        public string humidity { get; set; }
        public string temp_kf { get; set; }
    }

    [Serializable]
    public class Weather
    {
        public string id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    [Serializable]
    public class Clouds
    {
        public string all { get; set; }
    }

    [Serializable]
    public class Wind
    {
        public string speed { get; set; }
        public string deg { get; set; }
        public string gust { get; set; }
    }

    [Serializable]
    public class Sys
    {
        public string pod { get; set; }
    }

    [Serializable]
    public class DetailsList
    {
        public string dt { get; set; }
        public Main main { get; set; }
        public List<Weather> weather { get; set; }
        public Clouds clouds { get; set; }
        public Wind wind { get; set; }
        public string visibility { get; set; }
        public string pop { get; set; }
        public Sys sys { get; set; }
        public string dt_txt { get; set; }
    }

    [Serializable]
    public class Coord
    {
        public string lat { get; set; }
        public string lon { get; set; }
    }

    [Serializable]
    public class City
    {
        public string id { get; set; }
        public string name { get; set; }
        public Coord coord { get; set; }
        public string country { get; set; }
        public string population { get; set; }
        public string timezone { get; set; }
        public string sunrise { get; set; }
        public string sunset { get; set; }
    }

    [Serializable]
    public class RootObject
    {
        public string cod { get; set; }
        public string message { get; set; }
        public string cnt { get; set; }
        public List<DetailsList> list { get; set; }
        public City city { get; set; }
    }
}
