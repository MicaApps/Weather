using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWeather.Models
{
    public class WeatherClass
    {
        public string CityName { get; set; }
        public string State { get; set; }
        public string Temp { get; set; }
        public double FeelsLike { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public double Humidity { get; set; }
        public double Pressure { get; set; }

        public double Vsibility { get; set; }
        public double WindSpeed { get; set; }
        public double WindDegree { get; set; }
        public double Rainfall { get; set; }

        public long Sunrise { get; set; }
        public long Sunset { get; set; }

        public string Time { get; set; }
        public long TimeStamp { get; set; }

        public string UVDescription {  get; set; }

        public int UvIndex { get; set; }

        public int IconCode {  get; set; }

        public string MoonPhase {  get; set; }
        public string MoonPhaseCode {  get; set; }

        public int MoonPhaseDay {  get; set; }

        public string DayOrNight { get; set; }
    }

    public class SimpleWeatherClass
    {
        public string CityName { get; set; }
        public string State { get; set; }
        public double Temp { get; set; }
        public double MinTemp { get; set; }
        public double MaxTemp { get; set; }
        public long TimeStamp { get; set; }
        public string PlaceId { get; set; }

        /// <summary>
        /// 此处应当已经被转换为可绑定的路径
        /// </summary>
        public string IconImagePath { get; set; }
    }

    public class EveryDayWeather
    {
        public string CityId { get; set; }
        public string CityName { get; set; }
        public string Img { get; set; }
        public string Weather { get; set; }
        public string MinTemp { get; set; }
        public string MaxTemp { get; set; }
    }

    public class HourWeather
    {
        public string Time { get; set; }
        public double Temp { get; set; }
        public double TempMax { get; set; }
        public double TempMin { get; set; }


    }
}
