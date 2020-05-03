using PBGame.Rulesets;

namespace PBGame.Networking.Maps
{
    public class OnlineMap {
    
        /// <summary>
        /// Returns the identifier of the map.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Returns the name of the map.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Returns the game mode which the map is created for.
        /// </summary>
        public GameModeType Mode { get; set; }

        /// <summary>
        /// Returns the difficulty value calculated on the provider's side.
        /// </summary>
        public float Difficulty { get; set; }

        /// <summary>
        /// Returns the total duration of the map.
        /// </summary>
        public float TotalDuration { get; set; }

        /// <summary>
        /// Returns the hitting duration of the map.
        /// </summary>
        public float HitDuration { get; set; }

        /// <summary>
        /// Returns the bpm of the song.
        /// </summary>
        public float Bpm { get; set; }

        /// <summary>
        /// Returns the circle size of the hit objects.
        /// </summary>
        public float CS { get; set; }

        /// <summary>
        /// Returns the health drain difficulty.
        /// </summary>
        public float Drain { get; set; }

        /// <summary>
        /// Returns the hit difficulty.
        /// </summary>
        public float Accuracy { get; set; }

        /// <summary>
        /// Returns the approach rate of the hit objects.
        /// </summary>
        public float AR { get; set; }

        /// <summary>
        /// Returns the number of circles in the map.
        /// </summary>
        public int CircleCount { get; set; }

        /// <summary>
        /// Returns the slider count in the map.
        /// </summary>
        public int SliderCount { get; set; }

        /// <summary>
        /// Returns the spinner count in the map.
        /// </summary>
        public int SpinnerCount { get; set; }

        /// <summary>
        /// Returns the total hit objects mainly judged in the map.
        /// </summary>
        public int TotalCount { get; set; }
    }
}