using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppWeather.Models
{
    //如果好用，请收藏地址，帮忙分享。
    public class Loc
    {
        /// <summary>
        /// 
        /// </summary>
        public double lon { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double lat { get; set; }
    }

    public class CityInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string state { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Loc location { get; set; }
    }
}
