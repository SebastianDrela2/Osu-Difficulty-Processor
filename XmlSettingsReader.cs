using System.Xml.Linq;

namespace DifficultyProcessor;

internal static class XmlSettingsReader
{
    public static readonly string SettingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\OsuDifficultyParserSettings\settings.xml";

    public static OsuSettings GetOsuSettings()
    {
        var parentDirectory = Path.GetDirectoryName(SettingsPath)!;

        if (!Directory.Exists(parentDirectory))
        {
            Directory.CreateDirectory(parentDirectory);
        }

        if (!File.Exists(SettingsPath))
        {
            var initialXml = new XDocument(
                new XElement("Data",
                    new XElement("StartDirectory"),
                    new XElement("TargetFolder"),
                    new XElement("ApiKey"),
                    new XElement("DesiredDifficulty")
                )
            );

            initialXml.Save(SettingsPath);
        }

        var xDoc = XDocument.Load(SettingsPath);
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