namespace DifficultyProcessor.Settings
{
    public class OsuSettings
    {
        public static readonly string SettingsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\OsuDifficultyParserSettings\settings.xml";

        public string StartDirectory;
        public string TargetFolder;
        public string ApiKey;
        public int DesiredDifficulty;

        public OsuSettings(string startDirectory, string targetFolder, string apiKey, int desiredDifficulty)
        {
            StartDirectory = startDirectory;
            TargetFolder = targetFolder;
            ApiKey = apiKey;
            DesiredDifficulty = desiredDifficulty;
        }
    }
}
