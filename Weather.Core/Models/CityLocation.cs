namespace Weather.Core.Models;

public class CityLocation
{
    public string? Longitude { get; set; }

    public string? Latitude { get; set; }

    public string? Altitude { get; set; }

    public string? TimeZone { get; set; }

    public int UtcOffset { get; set; }

    public bool? IsDst { get; set; } = null;
}
