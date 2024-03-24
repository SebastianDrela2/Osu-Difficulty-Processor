using DifficultyProcessor.BeatmapManagement;
using DifficultyProcessor.Settings;
using DifficultyProcessor.XmlManagement;

namespace DifficultyProcessor
{
    internal class Program
    {
        private static readonly IList<string> _notResolvedHits = new List<string>();
        static void Main()
        {
            var searchPattern = "*.osu";
            var xmlSettingsReader = new XmlSettingsReader();
            var osuSettings = xmlSettingsReader.GetOsuSettings();

            if (osuSettings is null)
            {
                var xmlSettingSaver = new XmlSettingsSaver();
                xmlSettingSaver.SaveSettings();

                osuSettings = xmlSettingsReader.GetOsuSettings();
            }

            Console.WriteLine($"Loaded settings from: {OsuSettings.SettingsPath}");

            try
            {
                var foundFiles = Directory.GetFiles(osuSettings!.StartDirectory, searchPattern, SearchOption.AllDirectories);
                var allIDs = BeatMap.GetAllIds(foundFiles);
                var beatmapProcessor = new BeatmapProcessor(osuSettings.ApiKey, (int) osuSettings.DesiredMod, osuSettings.DesiredDifficulty, osuSettings.CheckIntervalInSeconds);

                ProcessFetchedIds(allIDs, beatmapProcessor, foundFiles, osuSettings);

                Console.ForegroundColor = ConsoleColor.White;
                DisplayNotResolvedHitsMessage();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Settings Path: {OsuSettings.SettingsPath}");
                throw; 
            }

            Console.ReadKey();
        }

        private static void DisplayNotResolvedHitsMessage()
        {
            var notResolvedHitsPath = @$"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\OsuDifficultyParserSettings\notResolvedHits.txt";

            File.WriteAllLines(notResolvedHitsPath, _notResolvedHits);
            Console.WriteLine($"Saved not resolved hits to {notResolvedHitsPath}");
            Console.WriteLine($"Total Count: {_notResolvedHits.Count}");            
        }

        private static void ProcessFetchedIds(List<string> allIDs, BeatmapProcessor beatmapProcessor, string[] foundFiles, OsuSettings osuSettings)
        {
            foreach (var id in allIDs)
            {
                var json = beatmapProcessor.GetJson(id);

                if (json is "Unauthorized")
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("UNAUTHORIZED ACCESS, WRONG API KEY");

                    return;
                }

                ProcessJson(beatmapProcessor, osuSettings, foundFiles, json);
            }
        }
        
        private static void ProcessJson(BeatmapProcessor beatmapProcessor, OsuSettings osuSettings, string[] foundFiles, string json)
        {
            if (json is null)
            {
                return;
            }

            var hit = beatmapProcessor.GetFullTitle(json);

            if (string.IsNullOrEmpty(hit))
            {
                return;
            }

            var pathHit = foundFiles.FirstOrDefault(x => x.Contains(hit));

            if (pathHit is null)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine($"Did not copy: {hit}, map not found in files, added to not resolved list.");
                _notResolvedHits.Add(hit);

                return;
            }

            var fileName = Path.GetFileName(pathHit);

            try
            {
                File.Copy(pathHit, @$"{osuSettings.TargetFolder}\{fileName}", true);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Copied {fileName}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Exception : {ex.Message} ");
                Console.ReadKey();
            }
        }
    }
}