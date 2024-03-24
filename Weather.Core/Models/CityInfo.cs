namespace Weather.Core.Models;

public class CityInfo
{
    public string? OriginData { get; set; }

    public string? Name { get; set; }

    public string? Country { get; set; }

    public string AdministrativeDivision => $"{Name}, {Country}";

    public string? Id { get; set; }

    public string? Rank { get; set; }

    public CityLocation Location { get; set; } = new();

    public DataReference Reference { get; set; } = new DataReference().InitializeDefault();
}
