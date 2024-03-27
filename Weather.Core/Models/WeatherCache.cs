namespace Weather.Core.Models;

public class WeatherCache
{
    public string Location { get; set; } = string.Empty;

    public WeatherInfo WeatherInfo { get; set; } = new();

    public DateTime CacheTime { get; set; } = DateTime.Now;

    public DateTime ExpireTime { get; set; } = DateTime.Now.AddMinutes(10);

    public bool IsExpired => DateTime.Now > ExpireTime;

    public WeatherCache Refresh(WeatherInfo? info = null)
    {
        if (info is not null)
            WeatherInfo = info;

        CacheTime = DateTime.Now;
        ExpireTime = DateTime.Now.AddMinutes(10);

        return this;
    }
}
