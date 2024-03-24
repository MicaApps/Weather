using Weather.Core.Models.Units;

namespace Weather.Core.Models;

public class WeatherInfo
{
    public DateTime ObservationTime { get; set; }

    public DateTime UpdateTime { get; set; }

    public DateTime ForecastTime { get; set; }

    public string Text { get; set; } = string.Empty;

    public Temperature? Temperature { get; set; }

    public Temperature? TemperatureMax { get; set; }

    public Temperature? TemperatureMin { get; set; }

    public Temperature? FeelsLike { get; set; }

    public DateTime? Sunrise { get; set; }

    public DateTime? Sunset { get; set; }

    public DateTime? Moonrise { get; set; }

    public DateTime? Moonset { get; set; }

    public string? MoonPhase { get; set; }

    public double Wind360 { get; set; }

    public double Wind360Day { get; set; }

    public double Wind360Light { get; set; }

    public string? WindDirection { get; set; }

    public string? WindDirectionDay { get; set; }

    public string? WindDirectionLight { get; set; }

    public double WindScale { get; set; }

    public double WindScaleDay { get; set; }

    public double WindScaleNight { get; set; }

    public double WindSpeedKmPerHour { get; set; }

    public double WindSpeedKmPerHourDay { get; set; }

    public double WindSpeedKmPerHourNight { get; set; }

    public string WindSpeedKmPerHourText => $"{WindSpeedKmPerHour} km/h";

    public double Humidity { get; set; }

    public string HumidityText => $"{Humidity} %";

    public double PrecipMillimeter { get; set; }

    public string PrecipMillimeterText => $"{PrecipMillimeter} mm";

    public double PressureHpa { get; set; }

    public string PressureHpaText => $"{PressureHpa} Hpa";

    public double VisibilityKm { get; set; }

    public string VisibilityKmText => $"{VisibilityKm} km";

    public double Cloud { get; set; }

    public double Dew { get; set; }

    public double UvIndex { get; set; }

    public DataReference Reference { get; set; } = new DataReference().InitializeDefault();
}
