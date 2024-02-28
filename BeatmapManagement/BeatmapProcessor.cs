using Newtonsoft.Json;

namespace DifficultyProcessor.BeatmapManagement
{
    public class BeatmapData
    {
        public string? Artists { get; set; }

        public string? Title { get; set; }

        public string? Version { get; set; }

        public string? Creator { get; set; }

        public double DifficultyRating { get; set; }
    }

    internal class BeatmapProcessor
    {
        private readonly string _clientSecret;
        private readonly string _modId;
        private readonly int _desiredDifficulty;

        public BeatmapProcessor(string clientSecret, string modId, int desiredDifficulty)
        {
            _clientSecret = clientSecret;
            _modId = modId;
            _desiredDifficulty = desiredDifficulty;
        }

        public string GetFullTitle(string json)
        {
            var fullTitle = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                var data = GetJsonBeatMapData(json).FirstOrDefault();

                if (data != null && data.DifficultyRating != 0)
                {
                    var diffOneHigherThanDesired = _desiredDifficulty + 1;

                    if (data.DifficultyRating > _desiredDifficulty && data.DifficultyRating < diffOneHigherThanDesired)
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

        private IEnumerable<BeatmapData> GetJsonBeatMapData(string json)
        {
            var obj = JsonConvert.DeserializeObject<List<BeatmapData>>(json);
            return obj ?? new List<BeatmapData>();
        }

        public string GetJson(string id)
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

        private async Task<string> GetOsuUrlJsonCall(string mapId)
        {
            try
            {
                using var client = new HttpClient();

                var apiUrl = $"https://osu.ppy.sh/api/get_beatmaps?k={_clientSecret}&b={mapId}mods={_modId}";

                Thread.Sleep(1000);

                var response = await client.GetAsync(apiUrl);
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Blue;

                var apiUrlWithHiddenKey = apiUrl.Replace(_clientSecret, "api_key");
                Console.WriteLine($"Checked this map: {apiUrlWithHiddenKey}");

                if (response.ReasonPhrase is "Unauthorized")
                {
                    return response.ReasonPhrase;
                }

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
