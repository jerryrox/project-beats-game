using PBGame.Rulesets;
using Newtonsoft.Json;

namespace PBGame.Networking.Maps
{
    public class OnlineMap {
    
        /// <summary>
        /// Returns the identifier of the map.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Returns the name of the map.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Returns the index of the game mode which the map is created for.
        /// </summary>
        [JsonProperty("mode")]
        public int ModeIndex { get; set; }

        /// <summary>
        /// Returns the game mode this maps is created for.
        /// </summary>
        [JsonIgnore]
        public GameModeType GameMode => (GameModeType)ModeIndex;

        /// <summary>
        /// Returns the difficulty value calculated on the provider's side.
        /// </summary>
        [JsonProperty("difficulty")]
        public float Difficulty { get; set; }

        /// <summary>
        /// Returns the total duration of the map.
        /// </summary>
        [JsonProperty("totalDuration")]
        public float TotalDuration { get; set; }

        /// <summary>
        /// Returns the hitting duration of the map.
        /// </summary>
        [JsonProperty("hitDuration")]
        public float HitDuration { get; set; }

        /// <summary>
        /// Returns the bpm of the song.
        /// </summary>
        [JsonProperty("bpm")]
        public float Bpm { get; set; }

        /// <summary>
        /// Returns the circle size of the hit objects.
        /// </summary>
        [JsonProperty("cs")]
        public float CS { get; set; }

        /// <summary>
        /// Returns the health drain difficulty.
        /// </summary>
        [JsonProperty("drain")]
        public float Drain { get; set; }

        /// <summary>
        /// Returns the hit difficulty.
        /// </summary>
        [JsonProperty("accuracy")]
        public float Accuracy { get; set; }

        /// <summary>
        /// Returns the approach rate of the hit objects.
        /// </summary>
        [JsonProperty("ar")]
        public float AR { get; set; }

        /// <summary>
        /// Returns the number of circles in the map.
        /// </summary>
        [JsonProperty("circleCount")]
        public int CircleCount { get; set; }

        /// <summary>
        /// Returns the slider count in the map.
        /// </summary>
        [JsonProperty("sliderCount")]
        public int SliderCount { get; set; }

        /// <summary>
        /// Returns the spinner count in the map.
        /// </summary>
        [JsonProperty("spinnerCount")]
        public int SpinnerCount { get; set; }

        /// <summary>
        /// Returns the total hit objects mainly judged in the map.
        /// </summary>
        [JsonProperty("totalCount")]
        public int TotalCount { get; set; }
    }
}