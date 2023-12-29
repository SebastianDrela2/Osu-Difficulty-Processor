using System.Text.RegularExpressions;

namespace DifficultyProcessor
{
    internal static class BeatMap
    {
        public static List<string> GetAllIds(string[] foundFiles)
        {
            var ids = new List<string>();

            foreach (var file in foundFiles)
            {
                try
                {
                    var id = GetBeatMapID(file);

                    if (id != string.Empty)
                    {
                        ids.Add(id);
                    }
                }
                catch
                {
                    // if anything happens, just continue
                }
            }

            return ids;
        }


        private static string GetBeatMapID(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            var lines = File.ReadAllLines(filePath);
            var pattern = @"^BeatmapID:(\d+)$";

            foreach (var line in lines)
            {
                var match = Regex.Match(line, pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return string.Empty;
        }
    }
}
