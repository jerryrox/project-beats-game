using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using PBGame.Rulesets;
using PBGame.Networking.Maps;
using PBFramework.Debugging;
using PBFramework.Networking.API;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PBGame.Networking.API.Osu.Responses
{
    public class MapsetListResponse : BaseResponse {

        private Action onParsed;

        /// <summary>
        /// Target number of tasks to await for.
        /// </summary>
        private int targetTaskCount = 0;

        /// <summary>
        /// Current number of tasks finished.
        /// </summary>
        private int curTaskCount = 0;


        /// <summary>
        /// Returns the array of mapsets retrieved from the server.
        /// </summary>
        public OnlineMapset[] Mapsets { get; private set; }

        /// <summary>
        /// Returns the cursor date for querying next page.
        /// </summary>
        public int CursorDate { get; private set; }

        /// <summary>
        /// Returns the cursor id for querying next page.
        /// </summary>
        public int CursorId { get; private set; }

        /// <summary>
        /// Returns the total results of the search.
        /// </summary>
        public int Total { get; private set; }


        public MapsetListResponse(IHttpRequest request, Action onParsed) : base(request)
        {
            this.onParsed = onParsed;
        }

        public override void Evaluate()
        {
            IsSuccess = request.Response.Code == 200;
            if(!IsSuccess)
                ErrorMessage = request.Response.ErrorMessage;
            // If successful, start parsing the result.
            else
            {
                try
                {
                    var json = JsonConvert.DeserializeObject<JObject>(request.Response.TextData);
                    {
                        var beatmapsets = json["beatmapsets"].ToObject<JArray>();
                        {
                            Mapsets = new OnlineMapset[beatmapsets.Count];
                            if (Mapsets.Length > 0)
                            {
                                int mapsetPerTask = 10;
                                targetTaskCount = (beatmapsets.Count - 1) / mapsetPerTask + 1;
                                for (int i = 0; i < targetTaskCount; i++)
                                    ParseMapsets(beatmapsets, i * mapsetPerTask, Math.Min((i + 1) * mapsetPerTask, Mapsets.Length - 1));
                            }
                            else
                            {
                                onParsed?.Invoke();
                            }
                        }
                        var cursor = json["cursor"];
                        {
                            if (int.TryParse(cursor["approved_date"].ToString(), out int cursorDate))
                                this.CursorDate = cursorDate;
                            if (int.TryParse(cursor["_id"].ToString(), out int cursorId))
                                this.CursorId = cursorId;
                        }
                        Total = json["total"].Value<int>();
                    }
                }
                catch (Exception e)
                {
                    Logger.LogError($"MapsetListResponse.Evaluate - Error while parsing response: {e.Message}\n{e.StackTrace}");
                }
            }
        }

        public override void ApplyResponse(IApi api)
        {
            // Nothing to do!
        }

        private Task ParseMapsets(JArray beatmapsets, int fromInx, int toInx)
        {
            return Task.Run(() =>
            {
                for (int i = fromInx; i <= toInx; i++)
                {
                    JObject rawData = beatmapsets[i].ToObject<JObject>();
                    OnlineMapset mapset = new OnlineMapset()
                    {
                        Id = rawData["id"].Value<int>(),
                        Title = rawData["title"].ToString(),
                        Artist = rawData["artist"].ToString(),
                        Creator = rawData["creator"].ToString(),
                        Source = rawData["source"].ToString(),
                        Tags = rawData["tags"].ToString(),
                        CoverImage = rawData["covers"]["cover"].ToString(),
                        CardImage = rawData["covers"]["card"].ToString(),
                        PreviewAudio = $"https:{rawData["preview_url"].ToString()}",
                        HasVideo = rawData["video"].Value<bool>(),
                        HasStoryboard = rawData["storyboard"].Value<bool>(),
                        Bpm = rawData["bpm"].Value<float>(),
                        PlayCount = rawData["play_count"].Value<int>(),
                        FavoriteCount = rawData["favourite_count"].Value<int>(),
                        LastUpdate = rawData["last_updated"].Value<DateTime>(),
                        Status = ParseStatus(rawData["status"].ToString()),
                        IsDisabled = rawData["availability"]["download_disabled"].Value<bool>(),
                        DisabledInformation = rawData["availability"]["more_information"].ToString()
                    };
                    Mapsets[i] = mapset;

                    // Parse maps
                    var beatmaps = rawData["beatmaps"].ToObject<JArray>();
                    foreach (var mapEntry in beatmaps)
                    {
                        var rawMap = mapEntry.ToObject<JObject>();
                        mapset.Maps.Add(new OnlineMap()
                        {
                            Id = rawMap["id"].Value<int>(),
                            Version = rawMap["version"].ToString(),
                            Mode = ParseMode(rawMap["mode_int"].Value<int>()),
                            Difficulty = rawMap["difficulty_rating"].Value<float>(),
                            TotalDuration = rawMap["total_length"].Value<float>(),
                            HitDuration = rawMap["hit_length"].Value<float>(),
                            Bpm = rawMap["bpm"].Value<float>(),
                            CS = rawMap["cs"].Value<float>(),
                            Drain = rawMap["drain"].Value<float>(),
                            Accuracy = rawMap["accuracy"].Value<float>(),
                            AR = rawMap["ar"].Value<float>(),
                            CircleCount = rawMap["count_circles"].Value<int>(),
                            SliderCount = rawMap["count_sliders"].Value<int>(),
                            SpinnerCount = rawMap["count_spinners"].Value<int>(),
                            TotalCount = rawMap["count_total"].Value<int>()
                        });
                    }
                }

                // If all parsing finished
                if (Interlocked.Increment(ref curTaskCount) == targetTaskCount)
                {
                    // Do parse finished callback
                    onParsed?.Invoke();
                }
            });
        }

        /// <summary>
        /// Parses the specified status string into MapStatus enum value.
        /// </summary>
        private MapStatus ParseStatus(string status)
        {
            foreach (var s in (MapStatus[])Enum.GetValues(typeof(MapStatus)))
            {
                if(status.Equals(s.ToString(), StringComparison.OrdinalIgnoreCase))
                    return s;
            }
            Logger.LogWarning($"MapsetListResponse.ParseStatus - Unknown status name: {status}");
            return MapStatus.Graveyard;
        }

        /// <summary>
        /// Parses the specified osu game mode index into GameModes enum value.
        /// </summary>
        private GameModes ParseMode(int modeIndex) => (GameModes)(modeIndex + GameProviders.Osu);
    }
}