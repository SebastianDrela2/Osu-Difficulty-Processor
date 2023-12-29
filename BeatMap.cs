using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RadiApplication
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
                catch(Exception)
                {
                    //
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
