using System.Xml.Linq;
using DifficultyProcessor.Settings;

namespace DifficultyProcessor.XmlManagement;

internal class XmlSettingsReader
{   
    public OsuSettings? GetOsuSettings()
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
        var desiredOsuMod = GetOsuMod(dataElement?.Element("DesiredMod")?.Value!);
        var checkIntervalInSeconds = int.Parse(dataElement?.Element("CheckIntervalInSeconds")?.Value!);
        var desiredDifficulty = 0;

        if (desiredDifficultyValue != null && int.TryParse(desiredDifficultyValue, out var parsedValue))
        {
            desiredDifficulty = parsedValue;
        }

        var osuSettings = new OsuSettings(startDirectory!, targetFolder!, apiKey!, desiredDifficulty, desiredOsuMod, checkIntervalInSeconds);

        return osuSettings;
    }

    private OsuMod GetOsuMod(string xElement)
    {
        return xElement switch
        {
            "None" => OsuMod.None,
            "NoFail" => OsuMod.NoFail,
            "Easy" => OsuMod.Easy,
            "Hidden" => OsuMod.Hidden,
            "HardRock" => OsuMod.HardRock,
            "SuddenDeath" => OsuMod.SuddenDeath,
            "DoubleTime" => OsuMod.DoubleTime,
            "Relax" => OsuMod.Relax,
            "HalfTime" => OsuMod.HalfTime,
            "Nightcore" => OsuMod.Nightcore,
            "Flashlight" => OsuMod.Flashlight,
            "Autoplay" => OsuMod.Autoplay,
            _ => throw new ArgumentException($"Invalid mod string: {xElement}")
        };
    }
}