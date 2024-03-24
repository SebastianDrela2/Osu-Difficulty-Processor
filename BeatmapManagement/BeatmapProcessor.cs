using Newtonsoft.Json;
using System.Net;

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
        private readonly int _modId;
        private readonly int _desiredDifficulty;
        private readonly int _checkIntervalInMiliSeconds;

        public BeatmapProcessor(string clientSecret, int modId, int desiredDifficulty, int checkIntervalInSeconds)
        {
            _clientSecret = clientSecret;
            _modId = modId;
            _desiredDifficulty = desiredDifficulty;
            _checkIntervalInMiliSeconds = checkIntervalInSeconds * 1000;
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

        public string? GetJson(string id)
        {
            var json = string.Empty;

            try
            {
                json = GetOsuUrlJsonCall(id).Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return json;
        }

        private async Task<string?> GetOsuUrlJsonCall(string mapId)
        {
            try
            {
                using var client = new HttpClient();

                var apiUrl = $"https://osu.ppy.sh/api/get_beatmaps?k={_clientSecret}&b={mapId}mods={_modId}";
              
                // We should wait couple seconds before calling api again.
                Thread.Sleep(_checkIntervalInMiliSeconds);

                var response = await client.GetAsync(apiUrl);
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Blue;

                var apiUrlWithHiddenKey = apiUrl.Replace(_clientSecret, "api_key");
                Console.WriteLine($"Checked this map: {apiUrlWithHiddenKey}");

                if (response.StatusCode is HttpStatusCode.Unauthorized)
                {
                    return response.StatusCode.ToString();
                }

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");

                return null;
            }

            return null;
        }
    }
}
