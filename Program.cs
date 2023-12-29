namespace DifficultyProcessor
{
    internal class Program
    {
        static void Main()
        {
            var searchPattern = "*.osu";
            var osuSettings = XmlSettingsReader.GetOsuSettings();

            if (osuSettings != null)
            {
                var foundFiles = Directory.GetFiles(osuSettings.StartDirectory, searchPattern,
                    SearchOption.AllDirectories);
                var allIDs = BeatMap.GetAllIds(foundFiles);

                var beatmapProcessor = new BeatmapProcessor(osuSettings.ApiKey, "0", osuSettings.DesiredDifficulty);
                foreach (var id in allIDs)
                {
                    var jsonOnline = beatmapProcessor.GetJson(id);

                    if (jsonOnline != string.Empty)
                    {
                        var hit = beatmapProcessor.GetFullTitle(jsonOnline);

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

                Console.ForegroundColor = ConsoleColor.White;
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine($"No settings were detected, created default settings in {XmlSettingsReader.SettingsPath}");
            }
        }

    }
}
