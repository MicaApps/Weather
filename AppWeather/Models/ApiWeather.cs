using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Net.Http;
using AppWeather.Views;
using Windows.UI.Xaml;
namespace AppWeather.Models
{

    public class WeatherAdapter
    {
        static HttpClient client = new HttpClient() { BaseAddress = new Uri(@"https://api.weather.com/") };
        private static string _systemLanguage;// = UserProfile.GlobalizationPreferences.Languages[0];


        public static SearchLocationResponse GetLocationInformation(string cityName)
        {
            string api = $"v3/location/searchflat?query={cityName}&language={CultureInfo.CurrentCulture.Name}&apiKey=793db2b6128c4bc2bdb2b6128c0bc230&format=json";
            var response = client.GetAsync(api).GetAwaiter().GetResult();
            if(response.IsSuccessStatusCode)
            {
                var responseString = response.Content.ReadAsStringAsync().Result;
                var ret = JsonConvert.DeserializeObject<SearchLocationResponse>(responseString);

                return ret;
            } 
            return null;
        }

        public static SearchLocationResponse GetLocationInformation(string cityName, string language)
        {
            string api = $"v3/location/searchflat?query={cityName}&language={language}&apiKey=793db2b6128c4bc2bdb2b6128c0bc230&format=json";
            var responseString = client.GetStringAsync(api).GetAwaiter().GetResult();
            var ret = JsonConvert.DeserializeObject<SearchLocationResponse>(responseString);
            //var placeId=ret.location[0].placeId;
            //placeId.Dump();
            return ret;
        }

        public static V3WxObservationsCurrent GetCurrentWeater(string placeId)
        {
            //_systemLanguage = CultureInfo.CurrentCulture.Name; 
            string api = "v2/aggcommon/v3-wx-observations-current;v3-wx-forecast-hourly-10day;v3-wx-forecast-daily-10day;v3-location-point;v2idxDrySkinDaypart10;v2idxWateringDaypart10;v2idxPollenDaypart10;v2idxRunDaypart10;v2idxDriveDaypart10?format=json&placeid="
                    + placeId
                    + "&units=" + "m" //Enum.GetName(typeof(WetUnits),0);
                    + "&language=" +
                    "zh-cn" + "&apiKey=793db2b6128c4bc2bdb2b6128c0bc230";

            var responseString = client.GetStringAsync(api).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<RootV3Response>(responseString).v3wxobservationscurrent;
        }

        public static V3WxObservationsCurrent GetCurrentWeater(string placeId,string Language)
        {
            string api = "v2/aggcommon/v3-wx-observations-current;v3-wx-forecast-hourly-10day;v3-wx-forecast-daily-10day;v3-location-point;v2idxDrySkinDaypart10;v2idxWateringDaypart10;v2idxPollenDaypart10;v2idxRunDaypart10;v2idxDriveDaypart10?format=json&placeid="
                    + placeId
                    + "&units=" + "m" //Enum.GetName(typeof(WetUnits),0);
                    + "&language=" +
                    Language + "&apiKey=793db2b6128c4bc2bdb2b6128c0bc230";
            var responseString = client.GetStringAsync(api).GetAwaiter().GetResult();
            return JsonConvert.DeserializeObject<RootV3Response>(responseString).v3wxobservationscurrent;
        }

        public static RootV3Response GetWeater(string placeId)
        {
            _systemLanguage = CultureInfo.CurrentCulture.Name;
            string api = "v2/aggcommon/v3-wx-observations-current;v3-wx-forecast-hourly-10day;v3-wx-forecast-daily-10day;v3-location-point;v2idxDrySkinDaypart10;v2idxWateringDaypart10;v2idxPollenDaypart10;v2idxRunDaypart10;v2idxDriveDaypart10?format=json&placeid="
                    + placeId
                    + "&units=" + "m" //Enum.GetName(typeof(WetUnits),0);
                    + "&language=" +
                    _systemLanguage + "&apiKey=793db2b6128c4bc2bdb2b6128c0bc230";

            try
            {
                var responseString = client.GetStringAsync(api).GetAwaiter().GetResult();
                return JsonConvert.DeserializeObject<RootV3Response>(responseString);
            }
            catch (Exception)
            {
                return null;
                //throw;
            }
        }

        //获取简易天气数据
        public static SimpleWeatherClass GetSimpleWeater(string placeId)
        {
            var v3Response = GetWeater(placeId);
            //v3Response.Dump();
            //v3Response.v3locationpoint.LocationV3.country +"-"+ v3Response.v3locationpoint.LocationV3.adminDistrict +"-"+ 
            //$"/WeatherIcons/{.IconImagePath}.png"
            return new SimpleWeatherClass() { CityName = v3Response.v3locationpoint.LocationV3.displayName, Temp = v3Response.v3wxobservationscurrent.temperature, State = v3Response.v3wxobservationscurrent.wxPhraseLong,PlaceId =placeId ,IconImagePath= $"/WeatherIcons/{v3Response.v3wxobservationscurrent.iconCode}.png" };
        }

        //获取详细天气数据
        public static List<WeatherClass> GetWeathers(string placeId)
        {
            List<WeatherClass> weathers = new List<WeatherClass>();

            var v3 = GetWeater(placeId);
            if (v3 == null) return null;

            //if (weatherInfo.cod.Equals("404")) return null;

            var hourly = v3.v3wxforecasthourly10day;
            var daily = v3.v3wxforecastdaily10day;
            //这里要获取未来几天的天气
            for (int i = 0; i < hourly.temperature.Count; i++)
            {
                var w=new WeatherClass(); 
                //需要把天气状态转换为可以识别的字符串和图片ID--未完成
                w.CityName = v3.v3locationpoint.LocationV3.displayName;//城市名称
                w.State = hourly.wxPhraseLong[i];//天气状态
                w.Temp = hourly.temperature[i].ToString();//温度
                w.FeelsLike =(double) hourly.temperatureFeelsLike[i];//体感温度
                w.MinTemp =(double) (daily.temperatureMin[i / 24]);//最小温度
                w.MaxTemp =(double) (daily.temperatureMax[i / 24]??0);//最大温度
                w.Humidity = hourly.relativeHumidity[i];//湿度
                w.Pressure = hourly.pressureMeanSeaLevel[i];//气压
                w.Vsibility = hourly.visibility[i];//能见度
                w.WindSpeed = hourly.windSpeed[i];//风速
                w.WindDegree = hourly.windDirection[i];//风向
                w.Rainfall = hourly.qpf[i];//降雨概率
                w.Time = hourly.validTimeLocal[i].ToString();//时间
                w.TimeStamp = hourly.validTimeUtc[i];//时间戳
                w.Sunrise =(long) v3.v3wxforecastdaily10day.sunriseTimeUtc[i/24];//日出时间
                w.Sunset = (long)v3.v3wxforecastdaily10day.sunsetTimeUtc[i/24];//日落时间
                w.UVDescription = daily.daypart[0].uvDescription[i/24+1]; 

                w.UvIndex = daily.daypart[0].uvIndex[i/24+0]??5;

                w.IconCode =daily.daypart[0].iconCode[i/24+1]??0;

                w.MoonPhase = daily.moonPhase[0];

                w.MoonPhaseCode = daily.moonPhaseCode[0];

                w.MoonPhaseDay = daily.moonPhaseDay[0];
                w.DayOrNight = v3.v3wxobservationscurrent.dayOrNight.ToString();//"N"表示晚上，"D"表示白天

                weathers.Add(w);

                //    在调试控制台输出内容
                //            System.Diagnostics.Debug.WriteLine("State：" + weatherInfo.list[i].weather[0].main +
                //                                                "\nMinTemp：" + (double.Parse(weatherInfo.list[i].main.temp_min) - 273.15).ToString() +
                //                                                "\nMaxTemp：" + (double.Parse(weatherInfo.list[i].main.temp_max) - 273.15).ToString() +
                //                                                "\nTime：" + weatherInfo.list[i].dt_txt);
            }






                return weathers;
        }
    }

    //list of units
    public enum WetUnits
    {
        m = 0, //metric
        e = 1, //imperial
        h = 2, //hybrid
        s = 3 //metric SI
    }

    public class Locale
    {
        public string locale1 { get; set; }
        public string locale2 { get; set; }
        public object locale3 { get; set; }
        public object locale4 { get; set; }
    }

    public class SearchedLocation
    {
        public string address { get; set; }
        public Locale locale { get; set; }
        public string displayName { get; set; }
        public string ianaTimeZone { get; set; }
        public string adminDistrict { get; set; }
        public object adminDistrictCode { get; set; }
        public string city { get; set; }
        public double longitude { get; set; }
        public string postalCode { get; set; }
        public double latitude { get; set; }
        public string placeId { get; set; }
        public object neighborhood { get; set; }
        public string country { get; set; }
        public string postalKey { get; set; }
        public string countryCode { get; set; }
        public bool disputedArea { get; set; }
        public object disputedCountries { get; set; }
        public object disputedCountryCodes { get; set; }
        public object disputedCustomers { get; set; }
        public List<bool> disputedShowCountry { get; set; }
        public string iataCode { get; set; }
        public string icaoCode { get; set; }
        public string locId { get; set; }
        public object locationCategory { get; set; }
        public string pwsId { get; set; }
        public string type { get; set; }
    }

    public class SearchLocationResponse
    {
        public List<SearchedLocation> location { get; set; }
    }


    public class Daypart
    {
        public List<int?> cloudCover { get; set; }
        public List<string> dayOrNight { get; set; }
        public List<string> daypartName { get; set; }
        public List<int?> iconCode { get; set; }
        public List<int?> iconCodeExtend { get; set; }
        public List<string> narrative { get; set; }
        public List<int?> precipChance { get; set; }
        public List<string> precipType { get; set; }
        public List<double?> qpf { get; set; }
        public List<double?> qpfSnow { get; set; }
        public List<string> qualifierCode { get; set; }
        public List<string> qualifierPhrase { get; set; }
        public List<int?> relativeHumidity { get; set; }
        public List<string> snowRange { get; set; }
        public List<int?> temperature { get; set; }
        public List<int?> temperatureHeatIndex { get; set; }
        public List<int?> temperatureWindChill { get; set; }
        public List<string> thunderCategory { get; set; }
        public List<int?> thunderIndex { get; set; }
        public List<string> uvDescription { get; set; }
        public List<int?> uvIndex { get; set; }
        public List<int?> windDirection { get; set; }
        public List<string> windDirectionCardinal { get; set; }
        public List<string> windPhrase { get; set; }
        public List<int?> windSpeed { get; set; }
        public List<string> wxPhraseLong { get; set; }
        public List<string> wxPhraseShort { get; set; }
    }


    public class LocationV3
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string city { get; set; }
        public Locale locale { get; set; }
        public string neighborhood { get; set; }
        public string adminDistrict { get; set; }
        public string adminDistrictCode { get; set; }
        public string postalCode { get; set; }
        public string postalKey { get; set; }
        public string country { get; set; }
        public string countryCode { get; set; }
        public string ianaTimeZone { get; set; }
        public string displayName { get; set; }
        public DateTimeOffset? dstEnd { get; set; }
        public DateTimeOffset? dstStart { get; set; }
        public string dmaCd { get; set; }
        public string placeId { get; set; }
        public bool disputedArea { get; set; }
        public object disputedCountries { get; set; }
        public object disputedCountryCodes { get; set; }
        public object disputedCustomers { get; set; }
        public List<bool> disputedShowCountry { get; set; }
        public string canonicalCityId { get; set; }
        public string countyId { get; set; }
        public string locId { get; set; }
        public object locationCategory { get; set; }
        public string pollenId { get; set; }
        public string pwsId { get; set; }
        public string regionalSatellite { get; set; }
        public object tideId { get; set; }
        public string type { get; set; }
        public string zoneId { get; set; }
    }

    public class RootV3Response
    {
        public string id { get; set; }

        [JsonProperty("v3-wx-observations-current")]
        public V3WxObservationsCurrent v3wxobservationscurrent { get; set; }

        [JsonProperty("v3-wx-forecast-daily-15day")]
        public V3WxForecastDaily v3wxforecastdaily15day { get; set; }

        [JsonProperty("v3-wx-forecast-daily-10day")]
        public V3WxForecastDaily v3wxforecastdaily10day { get; set; }

        [JsonProperty("v3-wx-forecast-hourly-10day")]
        public V3WxForecastHourly v3wxforecasthourly10day { get; set; }

        public object v3alertsHeadlines { get; set; }

        [JsonProperty("v3-location-point")] public V3LocationPoint v3locationpoint { get; set; }

        [JsonProperty("v2idxDriveDaypart10")] public V2idxDriveDaypartResult v2idxDriveDaypart10days { get; set; }

        [JsonProperty("v2idxRunDaypart10")] public V2idxRunDaypartResult v2idxRunDaypart10days { get; set; }

        [JsonProperty("v2idxPollenDaypart10")] public V2idxPollenDaypartResult v2idxPollenDaypart10days { get; set; }

        [JsonProperty("v2idxWateringDaypart10")]
        public V2idxWateringDaypartResult V2IdxWateringDaypart10days { get; set; }

        [JsonProperty("v2idxDrySkinDaypart10")]
        public V2idxDrySkinDaypartResult V2IdxDrySkinDaypart10days { get; set; }
    }


    public class V3WxForecastHourly
    {
        public List<int> cloudCover { get; set; }
        public List<string> dayOfWeek { get; set; }
        public List<string> dayOrNight { get; set; }
        public List<int> expirationTimeUtc { get; set; }
        public List<int> iconCode { get; set; }
        public List<int> iconCodeExtend { get; set; }
        public List<int> precipChance { get; set; }
        public List<string> precipType { get; set; }
        public List<double> pressureMeanSeaLevel { get; set; }
        public List<double> qpf { get; set; }
        public List<double> qpfSnow { get; set; }
        public List<int> relativeHumidity { get; set; }
        public List<int> temperature { get; set; }
        public List<int> temperatureDewPoint { get; set; }
        public List<int?> temperatureFeelsLike { get; set; }
        public List<int> temperatureHeatIndex { get; set; }
        public List<int> temperatureWindChill { get; set; }
        public List<string> uvDescription { get; set; }
        public List<int> uvIndex { get; set; }
        public List<DateTimeOffset> validTimeLocal { get; set; }
        public List<int> validTimeUtc { get; set; }
        public List<double> visibility { get; set; }
        public List<int> windDirection { get; set; }
        public List<string> windDirectionCardinal { get; set; }
        public List<int?> windGust { get; set; }
        public List<int> windSpeed { get; set; }
        public List<string> wxPhraseLong { get; set; }
        public List<string> wxPhraseShort { get; set; }
        public List<int> wxSeverity { get; set; }
    }


    public class RootStandaloneHourlyResponse
    {
        [JsonProperty("v3-wx-forecast-hourly-10day")]
        public V3WxForecastHourly v3wxforecasthourly10day { get; set; }
    }

    public class V3LocationPoint
    {
        [JsonProperty("location")] public LocationV3 LocationV3 { get; set; }
    }

    public class V3WxForecastDaily
    {
        public List<int?> calendarDayTemperatureMax { get; set; }
        public List<int> calendarDayTemperatureMin { get; set; }
        public List<string> dayOfWeek { get; set; }
        public List<int> expirationTimeUtc { get; set; }
        public List<string> moonPhase { get; set; }
        public List<string> moonPhaseCode { get; set; }
        public List<int> moonPhaseDay { get; set; }
        public List<DateTimeOffset?> moonriseTimeLocal { get; set; }
        public List<int?> moonriseTimeUtc { get; set; }
        public List<DateTimeOffset?> moonsetTimeLocal { get; set; }
        public List<int?> moonsetTimeUtc { get; set; }
        public List<string> narrative { get; set; }
        public List<double?> qpf { get; set; }
        public List<double> qpfSnow { get; set; }
        public List<DateTimeOffset?> sunriseTimeLocal { get; set; }
        public List<int?> sunriseTimeUtc { get; set; }
        public List<DateTimeOffset?> sunsetTimeLocal { get; set; }
        public List<int?> sunsetTimeUtc { get; set; }
        public List<int?> temperatureMax { get; set; }
        public List<int> temperatureMin { get; set; }
        public List<DateTimeOffset> validTimeLocal { get; set; }
        public List<int> validTimeUtc { get; set; }
        public List<Daypart> daypart { get; set; }
    }


    public class V3WxObservationsCurrent
    {
        public int? cloudCeiling { get; set; }
        public string cloudCoverPhrase { get; set; }
        public string dayOfWeek { get; set; }
        public string dayOrNight { get; set; }
        public int expirationTimeUtc { get; set; }
        public int iconCode { get; set; }
        public int iconCodeExtend { get; set; }
        public object obsQualifierCode { get; set; }
        public object obsQualifierSeverity { get; set; }
        public double precip1Hour { get; set; }
        public double precip6Hour { get; set; }
        public double precip24Hour { get; set; }
        public double pressureAltimeter { get; set; }
        public double pressureChange { get; set; }
        public double pressureMeanSeaLevel { get; set; }
        public int pressureTendencyCode { get; set; }
        public string pressureTendencyTrend { get; set; }
        public int relativeHumidity { get; set; }
        public double snow1Hour { get; set; }
        public double snow6Hour { get; set; }
        public double snow24Hour { get; set; }
        public DateTimeOffset sunriseTimeLocal { get; set; }
        public int sunriseTimeUtc { get; set; }
        public DateTimeOffset sunsetTimeLocal { get; set; }
        public int sunsetTimeUtc { get; set; }
        public int temperature { get; set; }
        public int temperatureChange24Hour { get; set; }
        public int temperatureDewPoint { get; set; }
        public int temperatureFeelsLike { get; set; }
        public int temperatureHeatIndex { get; set; }
        public int temperatureMax24Hour { get; set; }
        public int temperatureMaxSince7Am { get; set; }
        public int temperatureMin24Hour { get; set; }
        public int temperatureWindChill { get; set; }
        public string uvDescription { get; set; }
        public int uvIndex { get; set; }
        public DateTimeOffset validTimeLocal { get; set; }
        public int validTimeUtc { get; set; }
        public double visibility { get; set; }
        public int windDirection { get; set; }
        public string windDirectionCardinal { get; set; }
        public int? windGust { get; set; }
        public int windSpeed { get; set; }
        public string wxPhraseLong { get; set; }
        public object wxPhraseMedium { get; set; }
        public object wxPhraseShort { get; set; }
    }


    //driving difficulty index

    public class DrivingDifficultyIndexDaypart
    {
        public List<int> fcstValid { get; set; }
        public List<DateTimeOffset> fcstValidLocal { get; set; }
        public List<string> dayInd { get; set; }
        public List<int> num { get; set; }
        public List<string> daypartName { get; set; }
        public List<int> drivingDifficultyIndex { get; set; }
        public List<string> drivingDifficultyCategory { get; set; }
    }

    public class Metadata
    {
        public string language { get; set; }
        public string transactionId { get; set; }
        public string version { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public int expireTimeGmt { get; set; }
        public int statusCode { get; set; }
    }

    public class V2idxDriveDaypartResult
    {
        public Metadata metadata { get; set; }

        [JsonProperty("drivingDifficultyIndex12hour")]
        public DrivingDifficultyIndexDaypart drivingDifficultyIndex12hour { get; set; }
    }


    //running index

    public class RunWeatherIndexDaypart
    {
        public List<int> fcstValid { get; set; }
        public List<DateTimeOffset> fcstValidLocal { get; set; }
        public List<string> dayInd { get; set; }
        public List<int> num { get; set; }
        public List<string> daypartName { get; set; }
        public List<int> longRunWeatherIndex { get; set; }
        public List<string> longRunWeatherCategory { get; set; }
        public List<int> shortRunWeatherIndex { get; set; }
        public List<string> shortRunWeatherCategory { get; set; }
    }

    public class V2idxRunDaypartResult
    {
        public Metadata metadata { get; set; }

        [JsonProperty("runWeatherIndex12hour")]
        public RunWeatherIndexDaypart RunWeatherIndexDaypart { get; set; }
    }


    //pollen index

    public class PollenForecastDaypart
    {
        public List<int> fcstValid { get; set; }
        public List<DateTime> fcstValidLocal { get; set; }
        public List<string> dayInd { get; set; }
        public List<int> num { get; set; }
        public List<string> daypartName { get; set; }
        public List<int> grassPollenIndex { get; set; }
        public List<string> grassPollenCategory { get; set; }
        public List<int> treePollenIndex { get; set; }
        public List<string> treePollenCategory { get; set; }
        public List<int> ragweedPollenIndex { get; set; }
        public List<string> ragweedPollenCategory { get; set; }
    }


    public class V2idxPollenDaypartResult
    {
        public Metadata metadata { get; set; }

        [JsonProperty("pollenForecast12hour")] public PollenForecastDaypart PollenForecastDaypart { get; set; }
    }


    //watering needs index

    public class V2idxWateringDaypartResult
    {
        public Metadata metadata { get; set; }

        [JsonProperty("wateringNeedsIndex12hour")]
        public WateringNeedsIndexDaypart WateringNeedsIndexDaypart { get; set; }
    }

    public class WateringNeedsIndexDaypart
    {
        public List<int> fcstValid { get; set; }
        public List<DateTime> fcstValidLocal { get; set; }
        public List<string> dayInd { get; set; }
        public List<int> num { get; set; }
        public List<string> daypartName { get; set; }
        public List<int> wateringNeedsIndex { get; set; }
        public List<string> wateringNeedsCategory { get; set; }
    }


    //dry skin index

    public class DrySkinIndexDaypart
    {
        public List<int> fcstValid { get; set; }
        public List<DateTime> fcstValidLocal { get; set; }
        public List<string> dayInd { get; set; }
        public List<int> num { get; set; }
        public List<string> daypartName { get; set; }
        public List<int> drySkinIndex { get; set; }
        public List<string> drySkinCategory { get; set; }
    }

    public class V2idxDrySkinDaypartResult
    {
        public Metadata metadata { get; set; }

        [JsonProperty("drySkinIndex12hour")] public DrySkinIndexDaypart DrySkinIndexDaypart { get; set; }
    }
}
