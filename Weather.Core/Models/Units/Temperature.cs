namespace Weather.Core.Models.Units;

public class Temperature
{
    private double KelvinValue { get; set; } = 0;

    public double Value { get; set; } = -273.15;

    public string Text => Unit switch
    {
        TemperatureUnits.Celsius => $"{Value} °C",
        TemperatureUnits.Kelvin => $"{Value} K",
        TemperatureUnits.Fahrenheit => $"{Value} °F",
        TemperatureUnits.Rankine => $"{Value} °R",
        TemperatureUnits.Delisle => $"{Value} °De",
        TemperatureUnits.Newton => $"{Value} °N",
        TemperatureUnits.Réaumur => $"{Value} °Ré",
        TemperatureUnits.Rømer => $"{Value} °Rø",
        _ => $"{Value} ({Unit})",
    };

    public TemperatureUnits Unit { get; set; } = TemperatureUnits.Celsius;

    public Temperature SetValue(double value, TemperatureUnits unit = TemperatureUnits.Celsius)
    {
        var valueToCheck = unit switch
        {
            TemperatureUnits.Celsius => value + 273.15,
            TemperatureUnits.Kelvin => value,
            TemperatureUnits.Fahrenheit => (value + 459.67) * (5 / 9),
            TemperatureUnits.Rankine => value * (5 / 9),
            TemperatureUnits.Delisle => 373.15 - (value * (2 / 3)),
            TemperatureUnits.Newton => 273.15 + (value * (100 / 33)),
            TemperatureUnits.Réaumur => value * (5 / 4) + 273.15,
            TemperatureUnits.Rømer => ((value - 7.5) * (40 / 21)) + 273.15,
            _ => throw new ArgumentOutOfRangeException(
                nameof(unit),
                unit,
                "Provide correct unit please."
            )
        };

        if (valueToCheck < 0)
            throw new ArgumentOutOfRangeException(
                nameof(value),
                valueToCheck,
                "Kelvin temperature should be bigger than zero."
            );

        KelvinValue = valueToCheck;

        Value = value;

        Unit = unit;

        return this;
    }

    public Temperature ExportValue(out double value, TemperatureUnits unit = TemperatureUnits.Celsius)
    {
        value = GetValue(unit);

        return this;
    }

    public double GetValue(TemperatureUnits unit = TemperatureUnits.Celsius) => unit switch
    {
        TemperatureUnits.Celsius => KelvinValue - 273.15,
        TemperatureUnits.Kelvin => KelvinValue,
        TemperatureUnits.Fahrenheit => (KelvinValue * (9 / 5)) - 459.67,
        TemperatureUnits.Rankine => KelvinValue * (9 / 5),
        TemperatureUnits.Delisle => (373.15 - KelvinValue) * (3 / 2),
        TemperatureUnits.Newton => (KelvinValue - 273.15) * (33 / 100),
        TemperatureUnits.Réaumur => (KelvinValue - 273.15) * (4 / 5),
        TemperatureUnits.Rømer => (KelvinValue - 273.15) * (21 / 40) + 7.5,
        _ => Double.NaN,
    };
}

public enum TemperatureUnits
{
    Celsius = 0,
    Kelvin = 1,
    Fahrenheit = 2,
    Rankine = 3,
    Delisle = 4,
    Newton = 5,
    Réaumur = 6,
    Rømer = 7,
}
