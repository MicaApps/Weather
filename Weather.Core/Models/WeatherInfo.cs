using Weather.Core.Models.Units;

namespace Weather.Core.Models;

public class WeatherInfo
{
    public DateTime ObservationTime { get; set; }

    public DateTime UpdateTime { get; set; }

    public Temperature? Temperature { get; set; }

    public double FeelsLike { get; set; }

    public double Wind360 { get; set; }

    public string? WindDirection { get; set; }

    public double WindScale { get; set; }

    public double WindSpeedKmPerHour { get; set; }

    public double Humidity { get; set; }

    public double PrecipMillimeter { get; set; }

    public double PressureHpa { get; set; }

    public double Cloud { get; set; }

    public double Dew { get; set; }

    public DataReference Reference { get; set; } = new DataReference().InitializeDefault();
}
