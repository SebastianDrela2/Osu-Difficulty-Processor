namespace DifficultyProcessor.Settings
{
    public class OsuSettings
    {
        public static readonly string SettingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\OsuDifficultyParserSettings\settings.xml";

        public string StartDirectory;
        public string TargetFolder;
        public string ApiKey;
        public int DesiredDifficulty;
        public OsuMod DesiredMod;
        public int CheckIntervalInSeconds;

        public OsuSettings
            (string startDirectory, string targetFolder, string apiKey, int desiredDifficulty, OsuMod desiredMod, int checkIntervalInSeconds)
        {
            StartDirectory = startDirectory;
            TargetFolder = targetFolder;
            ApiKey = apiKey;
            DesiredDifficulty = desiredDifficulty;
            DesiredMod = desiredMod;
            CheckIntervalInSeconds = checkIntervalInSeconds;
        }
    }
}
