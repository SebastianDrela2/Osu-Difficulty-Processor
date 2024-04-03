namespace DifficultyProcessor.Settings
{
    public class OsuSettings
    {
        public static readonly string SettingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\OsuDifficultyParserSettings\settings.xml";

        public string OsuSongsPath;
        public string ResultsPath;
        public string ApiKey;
        public int DesiredDifficulty;
        public OsuMod DesiredMod;
        public int CheckIntervalInSeconds;

        public OsuSettings
            (string osuSongsPath, string resultsPath, string apiKey, int desiredDifficulty, OsuMod desiredMod, int checkIntervalInSeconds)
        {
            OsuSongsPath = osuSongsPath;
            ResultsPath = resultsPath;
            ApiKey = apiKey;
            DesiredDifficulty = desiredDifficulty;
            DesiredMod = desiredMod;
            CheckIntervalInSeconds = checkIntervalInSeconds;
        }
    }
}
