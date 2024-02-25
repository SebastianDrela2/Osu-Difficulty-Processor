using System.Xml.Linq;
using DifficultyProcessor.Settings;

namespace DifficultyProcessor.XmlManagement;

internal static class XmlSettingsReader
{   
    public static OsuSettings? GetOsuSettings()
    {
        var parentDirectory = Path.GetDirectoryName(OsuSettings.SettingsPath)!;

        if (!Directory.Exists(parentDirectory))
        {
            Directory.CreateDirectory(parentDirectory);
        }

        if (!File.Exists(OsuSettings.SettingsPath))
        {
            return null;
        }

        var xDoc = XDocument.Load(OsuSettings.SettingsPath);
        var dataElement = xDoc.Element("Data");

        var startDirectory = dataElement?.Element("StartDirectory")?.Value;
        var targetFolder = dataElement?.Element("TargetFolder")?.Value;
        var apiKey = dataElement?.Element("ApiKey")?.Value;
        var desiredDifficultyValue = dataElement?.Element("DesiredDifficulty")?.Value;
        var desiredDifficulty = 0;

        if (desiredDifficultyValue != null && int.TryParse(desiredDifficultyValue, out var parsedValue))
        {
            desiredDifficulty = parsedValue;
        }

        var osuSettings = new OsuSettings(startDirectory!, targetFolder!, apiKey!, desiredDifficulty);

        return osuSettings;
    }
}