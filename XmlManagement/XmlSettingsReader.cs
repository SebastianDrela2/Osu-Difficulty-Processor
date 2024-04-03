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

        var osuSongsPath = dataElement?.Element("OsuSongsPath")?.Value!;
        var resultsPath = dataElement?.Element("ResultsPath")?.Value!;
        var apiKey = dataElement?.Element("ApiKey")?.Value!;
        var desiredDifficultyValue = dataElement?.Element("DesiredDifficulty")?.Value;
        var desiredOsuMod = GetOsuMod(dataElement?.Element("DesiredMod")?.Value!);
        var checkIntervalInSeconds = int.Parse(dataElement?.Element("CheckIntervalInSeconds")?.Value!);
        var desiredDifficulty = 0;

        if (desiredDifficultyValue != null && int.TryParse(desiredDifficultyValue, out var parsedValue))
        {
            desiredDifficulty = parsedValue;
        }

        var osuSettings = new OsuSettings(osuSongsPath, resultsPath, apiKey, desiredDifficulty, desiredOsuMod, checkIntervalInSeconds);

        return osuSettings;
    }

    private OsuMod GetOsuMod(string xElement)
    {
        xElement = xElement.ToLower();

        return xElement switch
        {
            "none" => OsuMod.None,
            "nofail" => OsuMod.NoFail,
            "easy" => OsuMod.Easy,
            "hidden" => OsuMod.Hidden,
            "hardrock" => OsuMod.HardRock,
            "suddendeath" => OsuMod.SuddenDeath,
            "doubletime" => OsuMod.DoubleTime,
            "relax" => OsuMod.Relax,
            "halftime" => OsuMod.HalfTime,
            "nightcore" => OsuMod.Nightcore,
            "flashlight" => OsuMod.Flashlight,
            "autoplay" => OsuMod.Autoplay,
            _ => throw new ArgumentException($"Invalid mod string: {xElement}")
        };
    }
}