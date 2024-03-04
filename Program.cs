using DifficultyProcessor.BeatmapManagement;
using DifficultyProcessor.Settings;
using DifficultyProcessor.XmlManagement;

namespace DifficultyProcessor
{
    internal class Program
    {
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
                Console.ReadKey();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine($"Settings Path: {OsuSettings.SettingsPath}");
                throw; 
            }
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
            if (!string.IsNullOrEmpty(json))
            {
                var hit = beatmapProcessor.GetFullTitle(json);

                if (!string.IsNullOrEmpty(hit))
                {
                    var pathHit = foundFiles.FirstOrDefault(x => x.Contains(hit));

                    if (!string.IsNullOrEmpty(pathHit))
                    {
                        var fileName = Path.GetFileName(pathHit);
                        try
                        {
                            File.Copy(pathHit, @$"{osuSettings.TargetFolder}\{fileName}", true);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Copied {fileName}");
                        }
                        catch
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine("Error");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
    }
}