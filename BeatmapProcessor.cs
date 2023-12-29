using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace RadiApplication
{
    public class BeatmapData
    {
        public string artists { get; set; }

        public string title { get; set; }

        public string version { get; set; }

        public string creator { get; set; }

        public double difficultyrating { get; set; }
    }

    internal static class BeatmapProcessor
    {
        private static string client_secret = "API-KEY-FOR-NOW";

        public static string GetFullTitle(string json)
        {
            var fullTitle = string.Empty;

            if (!string.IsNullOrEmpty(json))
            {
                var data = GetJsonBeatMapData(json).FirstOrDefault();

                if (data != null && data.difficultyrating != 0)
                {
                    if (data.difficultyrating is > 7 and < 8)
                    {
                        fullTitle = $"{data.artists} - {data.title} ({data.creator}) [{data.version}]";
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

        private static List<BeatmapData> GetJsonBeatMapData(string json)
        {
            var obj = JsonConvert.DeserializeObject<List<BeatmapData>>(json);
            if (obj != null)
            {
                return obj;
            }

            return new List<BeatmapData>();
        }

        public static string GetJson(string id)
        {
            var json = string.Empty;

            try
            {
                json = ReturnOsuUrlJson(id).Result;
            }
            catch (Exception)
            {
                //
            }

            return json;
        }

        private static async Task<string> ReturnOsuUrlJson(string id)
        {
            try
            {
                using var client = new HttpClient();

                const string modId = "0";
                // Specify the URL you want to request data from

                var apiUrl = $"https://osu.ppy.sh/api/get_beatmaps?k={client_secret}&b={id}mods={modId}";

                Thread.Sleep(1000);

                var response = await client.GetAsync(apiUrl);
                Console.WriteLine($"");
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Checked this map: {apiUrl}");
                // Check if the response status code indicates success
                if (response.IsSuccessStatusCode)
                {
                    // Read the JSON response as a string
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
