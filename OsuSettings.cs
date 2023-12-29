namespace DifficultyProcessor
{
    internal class OsuSettings
    {
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
