using System;
using System.IO;
using System.Text.Json;
using Windows.ApplicationModel.Core;

namespace Weather.App.Configuration;

public class AppConfig
{
    private static object _instance = new();

    public static AppConfig Instance
    {
        get
        {
            if (_instance is not AppConfig)
            {
                var fileName = Path.Combine(
                    Path.GetDirectoryName(Environment.ProcessPath),
                    "Weather.App.Config.json"
                );

                if (File.Exists(fileName))
                    _instance = JsonSerializer.Deserialize<AppConfig>(File.ReadAllText(fileName));
                else
                    _instance = new AppConfig().Save();

                CoreApplication.Exiting += (s, e) => AppConfig.Instance.Save();
            }

            return _instance as AppConfig;
        }
    }

    public AppConfig Save()
    {
        var fileName = Path.Combine(
            Path.GetDirectoryName(Environment.ProcessPath),
            "Weather.App.Config.json"
        );

        File.WriteAllText(
            fileName,
            JsonSerializer.Serialize(
                this,
                new JsonSerializerOptions()
                {
                    WriteIndented = true,
                    IncludeFields = true,
                }
            )
        );

        return this;
    }

    public string SelectedLocation { get; set; } = string.Empty;

    public int RanTime { get; set; } = 0;

    public ApiSettings Api { get; set; } = new();

    public class ApiSettings
    {
        public string Key { get; set; } = string.Empty;

        public string ProviderIdentity { get; set; } = string.Empty;
    }
}
