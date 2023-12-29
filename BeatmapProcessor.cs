using Newtonsoft.Json;

namespace DifficultyProcessor
{
    public class BeatmapData
    {
        public string? Artists { get; set; }

        public string? Title { get; set; }

        public string? Version { get; set; }

        public string? Creator { get; set; }

        public double DifficultyRating { get; set; }
    }

    internal static class BeatmapProcessor
    {
        private const string ClientSecret = "API-KEY-FOR-NOW";
        private const string ModId = "0";

        public static string GetFullTitle(string json)
        {
            var fullTitle = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                var data = GetJsonBeatMapData(json).FirstOrDefault();

                if (data != null && data.DifficultyRating != 0)
                {
                    if (data.DifficultyRating is > 7 and < 8)
                    {
                        fullTitle = $"{data.Artists} - {data.Title} ({data.Creator}) [{data.Version}]";
                    }
                }
            }

            if (fullTitle != string.Empty)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"{fullTitle} is a hit.");
            }
            return fullTitle;
        }

        private static IEnumerable<BeatmapData> GetJsonBeatMapData(string json)
        {
            var obj = JsonConvert.DeserializeObject<List<BeatmapData>>(json);
            return obj ?? new List<BeatmapData>();
        }

        public static string GetJson(string id)
        {
            var json = string.Empty;

            try
            {
                json = GetOsuUrlJsonCall(id).Result;
            }
            catch
            {
                // return empty json if anything fails.
            }

            return json;
        }

        private static async Task<string> GetOsuUrlJsonCall(string mapId)
        {
            try
            {
                using var client = new HttpClient();

                var apiUrl = $"https://osu.ppy.sh/api/get_beatmaps?k={ClientSecret}&b={mapId}mods={ModId}";

                Thread.Sleep(1000);

                var response = await client.GetAsync(apiUrl);
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Checked this map: {apiUrl}");
                
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
