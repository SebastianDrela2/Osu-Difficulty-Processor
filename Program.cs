namespace RadiApplication
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var startDirectory = @"C:\Users\Niki\AppData\Local\osu!\Songs";
            var targetFolder = @"C:\Users\Niki\Documents\7star";
            var searchPattern = "*.osu";

            var foundFiles = Directory.GetFiles(startDirectory, searchPattern, SearchOption.AllDirectories);
            var allIDs = BeatMap.GetAllIds(foundFiles);

            foreach (var id in allIDs)
            {
                var jsonOnline = BeatmapProcessor.GetJson(id);

                if (jsonOnline != string.Empty)
                {
                    var hit = BeatmapProcessor.GetFullTitle(jsonOnline);

                    if (!string.IsNullOrEmpty(hit))
                    {
                        var pathHit = foundFiles.FirstOrDefault(x => x.Contains(hit));

                        if (!string.IsNullOrEmpty(pathHit))
                        {
                            var pathWithoutPath = Path.GetFileName(pathHit);
                            try
                            {
                                File.Copy(pathHit, @$"{targetFolder}\{pathWithoutPath}", true);
                                Console.ForegroundColor = ConsoleColor.Green;
                                Console.WriteLine($"Copied {pathWithoutPath}");
                            }
                            catch (Exception)
                            {
                                Console.ForegroundColor = ConsoleColor.White;
                                Console.WriteLine($"Epic error");
                                Console.ReadKey();
                            }

                        }
                    }
                }
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.ReadKey();
        }

    }
}
