using DifficultyProcessor.BeatmapManagement;
using DifficultyProcessor.Settings;
using DifficultyProcessor.XmlManagement;
using System.Xml.Linq;

namespace DifficultyProcessor
{
    internal class Program
    {
        static void Main()
        {
            var searchPattern = "*.osu";
            var osuSettings = XmlSettingsReader.GetOsuSettings();

            if (osuSettings is null)
            {
                RequestSettings();
                osuSettings = XmlSettingsReader.GetOsuSettings();
            }

            try
            {
                var foundFiles = Directory.GetFiles(osuSettings!.StartDirectory, searchPattern, SearchOption.AllDirectories);
                var allIDs = BeatMap.GetAllIds(foundFiles);
                var beatmapProcessor = new BeatmapProcessor(osuSettings.ApiKey, "0", osuSettings.DesiredDifficulty);

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
        }

        private static void RequestSettings()
        {
            var initialXml = new XDocument(
             new XElement("Data",
                 new XElement("StartDirectory"),
                 new XElement("TargetFolder"),
                 new XElement("ApiKey"),
                 new XElement("DesiredDifficulty")
             )
         );

            foreach (var element in initialXml.Root.Elements())
            {
                Console.Write($"Enter {element.Name}: ");

                var inputValue = Console.ReadLine();
                element.Value = inputValue;
            }

            initialXml.Save(OsuSettings.SettingsPath);
        }
    }
}