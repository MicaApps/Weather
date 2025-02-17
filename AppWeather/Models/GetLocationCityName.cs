using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Globalization;

namespace AppWeather.Models
{
    public class GetLocationCityName
    {
        public async Task<string> GetIp()
        {
            try
            {
                using (var httpClient = new HttpClient())
                {

                    string ipInfo = await httpClient.GetStringAsync("https://httpbin.org/ip");

                    var ip = JsonConvert.DeserializeObject<Data>(ipInfo);
                    return ip.origin;
                }
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string GetRegionByIP(string ip)
        {
            try
            {
                // 构建请求URL
                string url = $"http://ip-api.com/json/{ip}?lang={CultureInfo.CurrentCulture.Name}";

                // 创建Web请求
                WebRequest request = WebRequest.Create(url);

                // 发送请求并获取响应
                using (WebResponse response = request.GetResponse())
                using (Stream dataStream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(dataStream))
                {
                    // 读取响应内容
                    string responseFromServer = reader.ReadToEnd();

                    // 解析JSON响应（需要使用JSON.NET或其他库）
                    // 示例中省略了JSON解析的步骤，直接返回了整个响应字符串
                    return responseFromServer;
                }
            }
            catch (Exception ex)
            {
                // 处理异常，例如日志记录或返回默认区域信息
                return "获取区域信息失败: " + ex.Message;
            }
        }

        public async Task<string> GetLocalCityName()
        {
            try
            {
                return JsonConvert.DeserializeObject<LocationModel>(GetRegionByIP(await GetIp())).city;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public class Data
        {
            public string origin { get; set; }
        }
        public class LocationModel
        {
            public string status { get; set; }
            public string country { get; set; }
            public string countryCode { get; set; }
            public string region { get; set; }
            public string regionName { get; set; }
            public string city { get; set; }
            public string zip { get; set; }
            public string lat { get; set; }
            public string lon { get; set; }
            public string timezone { get; set; }
            public string isp { get; set; }
            public string org { get; set; }
            [JsonProperty("as")]
            public string as1 { get; set; }
            public string query { get; set; }
        }
    }


}
